using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Acr.UserDialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

//using Amazon.SimpleNotificationService;
//using Amazon.SimpleNotificationService.Model;

namespace eLOSSTeam.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.

	// Handles SNS notifications

	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
			LoadApplication(new App());
			// if (options.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey))
			//            {
			//
			//                NSDictionary remoteNotification = options[UIApplication.LaunchOptionsRemoteNotificationKey] as NSDictionary;
			//                if (remoteNotification != null)
			//                {
			//                    //new UIAlertView(remoteNotification.AlertAction, remoteNotification.AlertBody, null, "OK", null).Show();
			//                }
			//            }



			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
								   UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
								   new NSSet());

				UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
				UIApplication.SharedApplication.RegisterForRemoteNotifications();
			}
			else
			{
				UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
				UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
			}

			App.ScreenHeight = (int)UIScreen.MainScreen.Bounds.Height;
			App.ScreenWidth = (int)UIScreen.MainScreen.Bounds.Width;

			return base.FinishedLaunching(app, options);
		}

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			ProcessNotification(userInfo, false);
		}

		public override async void RegisteredForRemoteNotifications(UIApplication application, NSData token)
		{
			var deviceToken = token.Description.Replace("<", "").Replace(">", "").Replace(" ", "");
			if (!string.IsNullOrEmpty(deviceToken))
			{
				System.Diagnostics.Debug.WriteLine("Device Token: " + deviceToken);
				await AmazonUtils.RegisterDevice(AmazonUtils.Platform.IOS, deviceToken);
			}
		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			Console.WriteLine(@"Failed to register for remote notification {0}", error.Description);
		}
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.

    // Handles SNS notifications

    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
           // if (options.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey))
//            {
//
//                NSDictionary remoteNotification = options[UIApplication.LaunchOptionsRemoteNotificationKey] as NSDictionary;
//                if (remoteNotification != null)
//                {
//                    //new UIAlertView(remoteNotification.AlertAction, remoteNotification.AlertBody, null, "OK", null).Show();
//                }
//            }

            

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                                   UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                                   new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }

            App.ScreenHeight = (int)UIScreen.MainScreen.Bounds.Height;
            App.ScreenWidth = (int)UIScreen.MainScreen.Bounds.Width;

            return base.FinishedLaunching(app, options);
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
			ProcessNotification (userInfo, false);
        }

        public override async void RegisteredForRemoteNotifications(UIApplication application, NSData token)
        {
            // Get current device token
            var DeviceToken = token.Description;
            if (!string.IsNullOrWhiteSpace(DeviceToken))
            {
                DeviceToken = DeviceToken.Trim('<').Trim('>');
            }

            // Get previous device token
            var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");

            // Has the token changed?
            if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(DeviceToken))
            {
                await AmazonUtils.RegisterDevice(AmazonUtils.Platform.IOS, DeviceToken);
            }

            // Save new device token 
            NSUserDefaults.StandardUserDefaults.SetString(DeviceToken, "PushDeviceToken");
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            new UIAlertView("Error registering push notifications", error.LocalizedDescription, null, "OK", null).Show();
            Console.WriteLine(@"Failed to register for remote notification {0}", error.Description);
        }

		/*{
    		aps =     {alert = "{\"Sender\": \"Green Tomato\", 
    							 \"Title\": \"New Message!\", 
							 	 \"Text\": \"test\", 
							 	 \"ToFrom\": \"-9185843048110265565\", 
							 	 \"Time\": \"2016-05-10 00:08:16.367161\", 
							 	 \"Subject\": \"Message\"}";
    				   };
		}*/

		void ProcessNotification(NSDictionary userInfo, bool fromFinishedLaunching)
		{
			if (null != userInfo && userInfo.ContainsKey(new NSString("aps")))
			{
				//Get the aps dictionary
				NSDictionary aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;

				string alert = string.Empty;
				if (aps.ContainsKey(new NSString("alert")))
					alert = (aps[new NSString("alert")] as NSString).ToString();


				var current = (App)Xamarin.Forms.Application.Current;
				try
				{
					SNSMessage msg = JsonConvert.DeserializeObject<SNSMessage>(alert);
					if (msg.Subject.Equals("Message"))
					{
						try
						{
							if (current.chatDisplayed())
							{
								current.parseMessageObject(JsonConvert.SerializeObject(msg));
							}
							else
							{
								if (!fromFinishedLaunching)
								{
									//Manually show an alert
									if (!string.IsNullOrEmpty(msg.Text))
									{
										UIAlertView avAlert = new UIAlertView("New Message!", msg.Sender + ": " + msg.Text, null, "OK", null);
										avAlert.Show();
									}
								}
							}
						}
						catch (NullReferenceException e)
						{
							if (!fromFinishedLaunching)
							{
								//Manually show an alert
								if (!string.IsNullOrEmpty(msg.Text))
								{
									UIAlertView avAlert = new UIAlertView("New Message!", msg.Sender + ": " + msg.Text, null, "OK", null);
									avAlert.Show();
								}
							}
						}
					}
					else if (msg.Subject.Equals("Handshake"))
					{
						try
						{
							if (current.ChatSelectionPageActive || current.ChatPageActive)
							{
								current.parseMessageObject(JsonConvert.SerializeObject(msg));
							}
							else
							{
								if (!fromFinishedLaunching)
								{
									//Manually show an alert
									if (!string.IsNullOrEmpty(msg.Sender))
									{
										UIAlertView avAlert = new UIAlertView("Incoming conversation", msg.Sender + ": Wants to talk.", null, "OK", null);
										avAlert.Show();
									}
								}
							}
						}
						catch (NullReferenceException e)
						{
							if (!fromFinishedLaunching)
							{
								//Manually show an alert
								if (!string.IsNullOrEmpty(msg.Sender))
								{
									UIAlertView avAlert = new UIAlertView("Incoming conversation", msg.Sender + ": Wants to talk.", null, "OK", null);
									avAlert.Show();
								}
							}
						}
					}
					else if (msg.Subject.Equals("HandshakeEnd"))
					{
						try
						{
							if (current.ChatSelectionPageActive || current.ChatPageActive)
							{
								current.parseMessageObject(JsonConvert.SerializeObject(msg));
							}
						}
						catch (NullReferenceException e)
						{

						}
					}
					else if (msg.Subject.Equals("Announcement"))
					{
						if (!fromFinishedLaunching)
						{
							//Manually show an alert
							if (!string.IsNullOrEmpty(msg.Text))
							{
								UIAlertView avAlert = new UIAlertView(msg.Title, msg.Text, null, "OK", null);
								avAlert.Show();
							}
						}
					}
					else {
						if (!fromFinishedLaunching)
						{
							//Manually show an alert
							if (!string.IsNullOrEmpty(msg.Text))
							{
								UIAlertView avAlert = new UIAlertView(msg.Title, msg.Text, null, "OK", null);
								avAlert.Show();
							}
						}
					}
				}
				catch (Exception ex)
				{
					if (!fromFinishedLaunching)
					{
						//Manually show an alert
						if (!string.IsNullOrEmpty(alert))
						{
							UIAlertView avAlert = new UIAlertView("Notification", "Someone is trying to contact you", null, "OK", null);
							avAlert.Show();
						}
					}
				}



			}
			else {
				UIAlertView avAlert = new UIAlertView("Notification", "Someone is trying to contact you", null, "OK", null);
				avAlert.Show();
			}
		}
	}
}

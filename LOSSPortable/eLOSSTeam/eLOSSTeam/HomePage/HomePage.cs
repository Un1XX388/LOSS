using System;
using System.IO;
using System.Diagnostics;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Specialized;
using Xamarin.Forms;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.Util;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Acr.UserDialogs;
using Plugin.TextToSpeech;
using System.ComponentModel;
using Plugin.Connectivity;

namespace eLOSSTeam
{
    public class HomePage : ContentPage
    {
        Label label1 = new Label();
        Label label2 = new Label();
        Frame labelFrame;
        CancellationTokenSource cts;

        public HomePage()
        {
            Color bg;
            if (!Constants.internetConnection)
            {
                terminateAppDisplay();
            }
            //sets the background color based on settings
            if (Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
                bg = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;
                bg = Colors.background;
            }

            Title = "Home";
            label1.Text = LoadQuotes().Message;             // inspirational quote retrieved from the server
            label1.FontSize = 20;
            label1.Style = new Style(typeof(Label))
            {
                BaseResourceKey = Device.Styles.SubtitleStyleKey,
                Setters = {
                new Setter { Property = Label.TextColorProperty,Value = Color.White },
                new Setter {Property = Label.FontAttributesProperty, Value = FontAttributes.Italic },
                new Setter {Property = Label.FontFamilyProperty, Value = "Times New Roman" },
                }
            };
            labelFrame = new Frame
            {
                Content = label1,
                OutlineColor = Color.White,
                BackgroundColor = bg,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
            };

            var logo = new Image { Aspect = Aspect.AspectFit };
            logo.Source = Device.OnPlatform(
            iOS: ImageSource.FromFile("App6.png"),
            Android: ImageSource.FromFile("logo2.png"),
            WinPhone: ImageSource.FromFile("App6.png"));
            logo.VerticalOptions = LayoutOptions.StartAndExpand;
            logo.HeightRequest = 200;

            //========================================== Most Helpful Resources ============================
            //Survivors of Suicide Handbook
            var SOS_link = new Button
            {
                Text = "Survivors of Suicide Handbook",
                TextColor = Colors.barBackground,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex("ffffe6"),
                HeightRequest = 40,
                WidthRequest = 300
            };
            SOS_link.Clicked += SOSLinkPressed;

            //Support group
            var sg_link = new Button
            {
                Text = "Support Groups",
                TextColor = Colors.barBackground,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex("ffffe6"),
                HeightRequest = 40,
                WidthRequest = 300
            };
            sg_link.Clicked += sgLinkPressed;

           
            StackLayout links = new StackLayout
            {
                Padding = new Thickness(0, Device.OnPlatform(30, 0, 0), 0, 0),
                Children = {
                              new BoxView() { Color = Color.Transparent, HeightRequest = 4  },
                                 new Label { Text = "Helpful Resources", TextColor = Color.White, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))},
                                 new StackLayout { Children = { new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.5  },
                                 SOS_link,
                                 new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.1  },
                                 sg_link },
                                },

                            },
                VerticalOptions = LayoutOptions.EndAndExpand,
            };

            var mainContent = new StackLayout
            {
                Padding = new Thickness(30, Device.OnPlatform(20, 0, 0), 30, 30),
                Children = {
                    new BoxView() { Color = Color.Transparent, HeightRequest = 5  },
                    logo,
                    new ContentView {
                        Content = labelFrame
                    },
                    links
                },
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            ScrollView content = new ScrollView
            {
                Content = mainContent,
                Orientation = ScrollOrientation.Vertical
            };
            this.Content = content;
            
        }

        private async void terminateAppDisplay()
        {
            //if(AmazonUtils.getQuotesList.Count == 0)
            //{
            //    await DisplayAlert("Error", "This application requires an internet connection to function properly", "OK");
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        if (Device.OS == TargetPlatform.Android)
            //        {
            //            DependencyService.Get<IAndroidMethods>().CloseApp();
            //        }
            //        else if (Device.OS == TargetPlatform.iOS)
            //        {

            //        }
            //    });
            //}
            Boolean connected = await IsConnected();
            if (connected == false)
            {
                await DisplayAlert("Error", "This application requires an internet connection to function properly", "OK");
            }


        }

        public async Task<bool> IsConnected()
        {
			System.Diagnostics.Debug.WriteLine("IsRemoteReachable value: " + (CrossConnectivity.Current.IsRemoteReachable("google.com")));
            return CrossConnectivity.Current.IsConnected && await CrossConnectivity.Current.IsRemoteReachable("google.com");
        }

        //========================================================= FUNCTIONS =========================================================

        //detects user location 
        async private Task getLocation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
				locator.AllowsBackgroundUpdates = true;
                locator.DesiredAccuracy = 100;
                locator.PositionChanged += OnPositionChanged;
                var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                var latitude = position.Latitude.ToString();
                var longtitude = position.Longitude.ToString();
                label2.Text = String.Format("Longitude: {0} Latitude: {1}", longtitude, latitude);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                label2.Text = "Unable to find location";
            }

        }

        async private void OnPositionChanged(object sender, PositionEventArgs e)
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
            var latitude = position.Latitude.ToString();
            var longtitude = position.Longitude.ToString();
            label2.Text = String.Format("Longitude: {0} Latitude: {1}", longtitude, latitude);
        }

        //navigate and display Survivors of Suicide Handbook on button press
        async void SOSLinkPressed(object sender, EventArgs e)
        {    
            // if TTS is enabled, read the name   
            if (Helpers.Settings.SpeechSetting == true)
            {
                CrossTextToSpeech.Current.Speak("Survivors of Suicide Handbook");
            }
        
            UserDialogs.Instance.ShowLoading("Loading..");


            WebView webview = new WebView();
            //http://stackoverflow.com/questions/2655972/how-can-i-display-a-pdf-document-into-a-webview
            //using google docs viewer
            String pdf = "http://www.suicidology.org/Portals/14/docs/Survivors/Loss%20Survivors/SOS_handbook.pdf";
            webview.Source = "http://drive.google.com/viewerng/viewer?embedded=true&url=" + pdf;
            
            
            await Navigation.PushAsync(new ContentPage()
            {   

                Title = "Survivors of Suicide Handbook",
                Content = webview

            });
            UserDialogs.Instance.HideLoading();

        }

        //navigate and display Support Groups AFSP page on button press
        async void sgLinkPressed(object sender, EventArgs e)
        {
            if (Helpers.Settings.SpeechSetting == true)
            {
                CrossTextToSpeech.Current.Speak("Find a Support Group");
            }

			String url = "http://afsp.org/find-support/ive-lost-someone/find-a-support-group/";
            UserDialogs.Instance.ShowLoading();

            WebView webView = new WebView
            {
				Source = url,
                VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
            };

			await Navigation.PushAsync(new ContentPage()
            {
                Title = "Find A Support Group",
                Content = webView

            });
            UserDialogs.Instance.HideLoading();

        }

        //Generate random quotes pulled from the server
        static Random rnd = new Random();
        
        private InspirationalQuote LoadQuotes()
        {
            var quoteList = AmazonUtils.getQuotesList;
            if (quoteList.Count == 0)
            {
                AmazonUtils.getQuotesList.CollectionChanged += new NotifyCollectionChangedEventHandler(quoteList_CollectionChanged);
                return new InspirationalQuote { Message = "" };
            }
            int size = quoteList.Count;
            int num = rnd.Next(0, size - 1);
            return quoteList[num];
        }

        //Display quote on homepage
        void quoteList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            label1.Text = LoadQuotes().Message;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            terminateAppDisplay();
        }

    }
}

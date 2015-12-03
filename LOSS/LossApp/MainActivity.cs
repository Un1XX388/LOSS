using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using System.Collections.Generic;
using Android.Locations;
using System.Threading;
using System.Text;
using System.Linq;
using System.Xml;

namespace LOSS
{
	[Activity (Label = "LOSS", MainLauncher = true, Icon = "@drawable/icon", Theme="@style/MyTheme")]
	public class MainActivity : ActionBarActivity, ILocationListener
	{
		private SupportToolbar mToolbar;
		private MyActionBarDrawerToggle mDrawerToggle;
		private DrawerLayout mDrawerLayout;
		private ListView mLeftDrawer;
		private ArrayAdapter mLeftAdapter;
		private List<string> mLeftDataSet;

		Location currentLocation;
		LocationManager locationManager;
		TextView locationText;
		TextView addressText;
		string locationProvider;

		/// <param name="location">The new location, as a Location object.</param>
		/// <summary>
		/// Called when the location has changed.
		/// </summary>
		public void OnLocationChanged(Location location)
		{
			try
			{
				currentLocation = location;

				if (currentLocation == null)
					locationText.Text = "Unable to determine your location.";
				else
				{
					locationText.Text = String.Format("{0},{1}", currentLocation.Latitude, currentLocation.Longitude);

					Geocoder geocoder = new Geocoder(this);

					//The Geocoder class retrieves a list of address from Google over the internet
					IList<Address> addressList =  geocoder.GetFromLocation(currentLocation.Latitude, currentLocation.Longitude, 10);

					Address address = addressList.FirstOrDefault();

					if (address != null)
					{
						StringBuilder deviceAddress = new StringBuilder();

						for (int i = 0; i < address.MaxAddressLineIndex; i++)
							deviceAddress.Append(address.GetAddressLine(i))
								.AppendLine(",");

						addressText.Text = deviceAddress.ToString();
					}
					else
						addressText.Text = "Unable to determine the address.";
				}
			}
			catch
			{
				addressText.Text = "Unable to determine the address.";
			}
		}
			
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
	
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			addressText = FindViewById<TextView>(Resource.Id.address_text);
			locationText = FindViewById<TextView>(Resource.Id.location_text);

			//Initialising the LocationManager to provide access to the system location services.
			//The LocationManager class will listen for GPS updates from the device and notify the application by way of events. 
			locationManager = (LocationManager)GetSystemService(LocationService);

			//Define a Criteria for the best location provider
			Criteria criteriaForLocationService = new Criteria{
				//A constant indicating an approximate accuracy
				Accuracy = Accuracy.Coarse,
				PowerRequirement = Power.Medium
			};

			IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);

			if (acceptableLocationProviders.Any())
				locationProvider = acceptableLocationProviders.First();
			else
				locationProvider = String.Empty;
			

			//Pull-out menu and menu items
			mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
			mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
			mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);

			mLeftDrawer.Tag = 0;

			SetSupportActionBar(mToolbar);
		
			mLeftDataSet = new List<string>();
			mLeftDataSet.Add ("Account Preferences");
			mLeftDataSet.Add ("Resources");
			mLeftDataSet.Add ("Chat");
			mLeftDataSet.Add ("Feedback");
			mLeftDataSet.Add ("Settings");
			mLeftDataSet.Add ("Hotline");

			//submenu of Resources Menu Item
			List<string> subResources = new List<string>();
			subResources.Add ("Web");
			subResources.Add ("Media");

			//submenu of Feedback Menu Item
			List<string> subFeedback = new List<string>();
			subFeedback.Add ("Report");

			mLeftAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, mLeftDataSet);
			mLeftDrawer.Adapter = mLeftAdapter;



			mDrawerToggle = new MyActionBarDrawerToggle(
				this,							//Host Activity
				mDrawerLayout,					//DrawerLayout
				Resource.String.openDrawer,		//Opened Message
				Resource.String.closeDrawer		//Closed Message
			);

			mDrawerLayout.SetDrawerListener(mDrawerToggle);
			SupportActionBar.SetHomeButtonEnabled(true);
			SupportActionBar.SetDisplayShowTitleEnabled(true);
			mDrawerToggle.SyncState();

			if (bundle != null)
			{
				if (bundle.GetString("DrawerState") == "Opened")
				{
					SupportActionBar.SetTitle(Resource.String.openDrawer);
				}

				else
				{
					SupportActionBar.SetTitle(Resource.String.closeDrawer);
				}
			}

			else
			{
				//This is the first time the activity is ran
				SupportActionBar.SetTitle(Resource.String.closeDrawer);
			}
		}



		public override bool OnOptionsItemSelected (IMenuItem item)
		{		
			switch (item.ItemId)
			{

			case Android.Resource.Id.Home:
				//The hamburger icon was clicked which means the drawer toggle will handle the event
				//all we need to do is ensure the right drawer is closed so the don't overlap
//				mDrawerLayout.CloseDrawer (mRightDrawer);
				mDrawerToggle.OnOptionsItemSelected(item);
				return true;

			case Resource.Id.action_refresh:
				//Refresh
				return true;

//			case Resource.Id.action_help:
//				if (mDrawerLayout.IsDrawerOpen(mRightDrawer))
//				{
//					//Right Drawer is already open, close it
//					mDrawerLayout.CloseDrawer(mRightDrawer);
//				}
//
//				else
//				{
//					//Right Drawer is closed, open it and just in case close left drawer
//					mDrawerLayout.OpenDrawer (mRightDrawer);
//					mDrawerLayout.CloseDrawer (mLeftDrawer);
//				}
//
//				return true;

			default:
				return base.OnOptionsItemSelected (item);
			}
		}
			
		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.action_menu, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			if (mDrawerLayout.IsDrawerOpen((int)GravityFlags.Left))
			{
				outState.PutString("DrawerState", "Opened");
			}

			else
			{
				outState.PutString("DrawerState", "Closed");
			}

			base.OnSaveInstanceState (outState);
		}

		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			mDrawerToggle.SyncState();
		}

		public override void OnConfigurationChanged (Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			mDrawerToggle.OnConfigurationChanged(newConfig);
		}

		/// <summary>
		/// Override OnResume so that Activity1 will begin listening to the LocationManager 
		/// when the activity comes into the foreground:
		/// </summary>
		protected override void OnResume()
		{
			base.OnResume();
			locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
		}

		/// <summary>
		/// Override OnPause and unsubscribe Activity1 from the LocationManager when the activity goes into the background:
		/// </summary>
		protected override void OnPause()
		{
			base.OnPause();
			locationManager.RemoveUpdates(this);
		}

		public void OnStatusChanged (string provider, Availability status, Android.OS.Bundle extras)
		{
		}

		public void OnProviderDisabled (string provider)
		{
		}

		public void OnProviderEnabled (string provider)
		{
		}

	}
}



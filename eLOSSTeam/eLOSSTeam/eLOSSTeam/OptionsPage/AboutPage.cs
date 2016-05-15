using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.TextToSpeech;
using Amazon.DynamoDBv2.DataModel;
using System.Collections.Specialized;

namespace eLOSSTeam
{
    public class AboutPage : ContentPage
    {
        Label aboutUs;
        public RangeObservableCollection<Miscellaneous> misc_items { get; set; }

        public AboutPage()
        {

            //sets the background color based on settings
            if (Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;
            }

            Title = "About Us";
            this.Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);


            misc_items = AmazonUtils.getMiscList;            // Holds data to be displayed on this content page.
            aboutUs = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                FontFamily = "Times New Roman",
                TextColor = Color.White              
            };

            System.Diagnostics.Debug.WriteLine("SIZE: " + misc_items.Count); 

            for(int i=0; i < misc_items.Count; i++)
            {
               if(misc_items[i].Type == "About Us")
                {
                    aboutUs.Text = misc_items[i].Description;
                }             
            }

            var maincontent = new StackLayout
            {
                Children = { aboutUs },
                Padding = 10

            };

            ScrollView content = new ScrollView
            {
                Content = maincontent,
                Orientation = ScrollOrientation.Vertical
            };
            this.Content = content;

        }

        //start speaking as soon as this page appears if tts is enabled
        protected override void OnAppearing()
        {
            if (Helpers.Settings.SpeechSetting == true)
            {
                CrossTextToSpeech.Current.Speak(aboutUs.Text);
            }

            base.OnAppearing();
        }




        //stop text to speech when navigation bar back button is pressed
        protected override void OnDisappearing()
        {
			System.Diagnostics.Debug.WriteLine ("inside OnDisppearing method");
			if (Device.OS == TargetPlatform.iOS) 
			{
				System.Diagnostics.Debug.WriteLine ("if targetplatform ios");

				Content = null;			
			}
            CrossTextToSpeech.Dispose();
			System.Diagnostics.Debug.WriteLine ("after dispose");

			base.OnDisappearing();
	
        }

        //==================================================== Back Button Pressed ==============================================================

        //stop text to speech when phone back button is pressed
        protected override Boolean OnBackButtonPressed() // back button pressed
        {
            CrossTextToSpeech.Dispose();
            ((RootPage)App.Current.MainPage).NavigateTo();
            return true;
        }
    }// end class AboutPage
   
}

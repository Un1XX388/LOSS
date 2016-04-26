using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.TextToSpeech;

namespace LOSSPortable
{
    public class AboutPage : ContentPage
    {
        Label aboutUs;
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

            aboutUs = new Label
            {
                Text = "\t\tThis app, e-LOSSteam, builds on Dr. Frank R. Campbell’s vision of “an active model of postvention…to let suicide survivors know that resources exist as soon as possible following the death” through a program he dubbed LOSS (Local Outreach to Suicide Survivors) Team"+
                        "\n\n\t\tThis app was conceived by Dr.Regina T.Praetorius, PhD, LMSW - AP after collaborating with Dr.Campbell and others in her community to build a LOSS Team.While establishing her community’s team," + 
                        "she became mindful of the resources a community needs to develop and sustain a LOSS Team—not all communities are in a position to do so.Thus, the goal of this app is to provide resources that a LOSS Team" + 
                        "might share for those in communities without a LOSS Team." +
                        "\n\n\t\tThe e-LOSSteam app was created by Reed D.Clanton, William K.Harrod, Matthew Meelhuysen, Isha Soni, and Idan Zoarets for a class project toward the completion" +
                        "of their undergraduate degrees in Computer Science/ Software Engineering. This project was under the direction of Dr.Christopher McMurrough. The logo was created by Xavia Kirk, as part of their work toward a degree in Computer Engineering.",
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                FontFamily = "Times New Roman",
                TextColor = Color.White
                
            };

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
            CrossTextToSpeech.Dispose();

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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    public class AboutPage : ContentPage
    {
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

            Label info = new Label
            {
                Text = "\t\t\tThis app, _________, builds on Dr. Frank R. Campbell’s vision of “an active model of postvention [activities that come after a suicide to alleviate its impact] made up of a team of trained survivors [those left behind to grieve the loss of someone to suicide] who would go to the scenes of suicides to disseminate information about resources and be the installation of hope for the newly bereaved. The primary goal of the APM is to let suicide survivors know that resources exist as soon as possible following the death.” In many communities, this team is known as the LOSS Team (Local Outreach to Suicide Survivors). In a typical situation, a suicide survivor will take up to 4 years to access help in grieving their loved one’s death by suicide—usually after much turmoil and distress. In communities where there is a LOSS Team, survivors usually access help within 30 days. The fact that not every community has a LOSS Team, nor the resources to sustain one, necessitates an app like this one." + 
                        "\n\n\t\t\t_________ was conceived by Dr. Regina T. Praetorius, PhD, LMSW-AP after collaborating with Dr. Campbell and others in her community to build a LOSS Team. Having been trained and mentored by Dr. Campbell for over a decade, and being acutely aware of how a community suffers after each suicide, she was dismayed at the thought of the pain sustained by so many communities without LOSS Teams. While establishing a LOSS Team in her own community, she became mindful of all the resources a community would need to develop and sustain a LOSS Team—not all communities are in a position to do so. Thus, the goal of this app is to fill in the gap for survivors who are not able to receive outreach from a LOSS Team. Thus, this app contains many of the resources that a LOSS Team might share." +
                        "\n\n\t\t\tThe _______ app was created by Reed D. Clanton, William K. Harrod, Matthew Jason Meelhuysen, Ishita Soni, and Idan Zoarets for a class project toward the completion of their undergraduate degrees in Computer Science. This project was under the direction of Dr. Christopher McMurrough. The logo was created by ___________, as part of their work toward a degree in __________."
            };

            var maincontent = new StackLayout
            {
                Children = { info },
                Padding = 10

            };

            ScrollView content = new ScrollView
            {
                Content = maincontent,
                Orientation = ScrollOrientation.Vertical
            };
            this.Content = content;

            info.FontSize = Device.GetNamedSize(NamedSize.Small, info);
            info.Style = new Style(typeof(Label))
            {
                BaseResourceKey = Device.Styles.BodyStyleKey,
                Setters = {
                new Setter { Property = Label.TextColorProperty,Value = Color.White },
                new Setter {Property = Label.FontFamilyProperty, Value = "Times New Roman" },

                }
       

            };
        }

        //functions would be outside of this
    }
}

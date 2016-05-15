using Xamarin.Forms;

namespace eLOSSTeam
{
    public class AccountPage: CarouselPage
    {

        public AccountPage()
        {
            Title = "Account";
            
            //set background color based on contrast setting
            if(Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;
            }

            
            if(Helpers.Settings.LoginSetting == true)
            {
                Children.Add(new RegisteredAccountPage()); //displays page similar to profile, an option to change password, general settings, logout option
            }
            else
            {
                Children.Add(new GeneralAccountPage()); //displays general settings, login or register option, report a problem option
            }
        }

        
    }
}

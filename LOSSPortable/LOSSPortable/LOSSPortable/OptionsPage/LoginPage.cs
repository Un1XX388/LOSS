using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    public class LoginPage: ContentPage
    {
        Boolean loggedIn = false;

        public LoginPage()
        {
           // BindingContext = new LoginViewModel(Navigation);

            var layout = new StackLayout { Padding = 10 };

            var layout2 = new StackLayout
            {
                BackgroundColor = Color.Gray, 
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            var username = new Entry { Placeholder = "Username" };
            layout.Children.Add(username);

            var password = new Entry { Placeholder = "Password", IsPassword = true };
            layout.Children.Add(password);

            Button Login = new Button { Text = "Log In", TextColor = Color.White };
            Login.Clicked += Login_Clicked;
            layout.Children.Add(Login);

            var label = new Label
            {
                Text = "Forgot Password?",
                FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center, // Center the text in the blue box.
                VerticalTextAlignment = TextAlignment.Center, // Center the text in the blue box.
            };
            label.GestureRecognizers.Add(new TapGestureRecognizer {
                Command = new Command(
                           () => ForgotPassword()),
            });

            layout.Children.Add(label);

            var label2 = new Label
            {
                Text = "or",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center, // Center the text in the blue box.
                VerticalTextAlignment = TextAlignment.Center, // Center the text in the blue box.
                HeightRequest = 20
            };

            layout.Children.Add(label2);

            Button signUpButton = new Button { Text = "Sign Up", TextColor = Color.White };
            signUpButton.Clicked += SignUpButton_Clicked;
            layout.Children.Add(signUpButton);

            //layout.Children.Add(layout2);
            this.Content = new ScrollView
            {
                Content = layout
            };

        }//LoginPage()

        public async void ForgotPassword()
        {
            var s = await UserDialogs.Instance.PromptAsync(new PromptConfig
            {
                Message = "Enter E-Mail"
            });
            var stat = s.Ok ? "Success" : "Cancelled";
        }

        private void SignUpButton_Clicked(object sender, EventArgs e)
        {
            login_check();
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            loggedIn = true;
            Helpers.Settings.LoginSetting = true;
            ((RootPage)App.Current.MainPage).NavigateTo();
        }

        public async void login_check()
        {
            if (loggedIn == false) //login is currently displayed. set loggedIn to true to display logout
            {
                System.Diagnostics.Debug.WriteLine("Login currently displayed - loggedIn" + loggedIn);
                //entry pop up for login
                var r = await UserDialogs.Instance.LoginAsync(new LoginConfig
                {
                    Message = "Enter Credentials",
                    LoginPlaceholder = Helpers.Settings.EmailSetting,
                    //  PasswordPlaceholder = Helpers.Settings.PasswordSetting
                });

                var status = r.Ok ? "Success" : "Cancelled";
                //  System.Diagnostics.Debug.WriteLine("after status = r.ok?");

                // this.Result($"Login {status} - User Name: {r.LoginText} - Password: {r.Password}");
                if (status == "Success")
                {
                    Helpers.Settings.EmailSetting = r.LoginText;
                    Helpers.Settings.PasswordSetting = r.Password;
                    loggedIn = true;
                  //  login.Text = "Logout";
                    System.Diagnostics.Debug.WriteLine("status is success then logout should be displayed - loggedIn" + loggedIn);
                    return;
                }
                else if (status == "Cancelled")
                {
                 //   login.Text = "Login";
                    System.Diagnostics.Debug.WriteLine("status is cancelled then login should be displayed - loggedIn" + loggedIn);
                    return;
                }
            }

            else if (loggedIn == true)//if logout is displayed
            {
                System.Diagnostics.Debug.WriteLine("Logout currently displayed - loggedIn" + loggedIn);

                //pop up confirmation
                var result = await DisplayAlert("Log Out", "Are you sure?", "Yes", "No");
                if (result == false)
                {
                    // loggedIn = true;
                    //login.Text = "Logout";
                    //event_label.Text = String.Format("Logout canceled");
                    System.Diagnostics.Debug.WriteLine("User clicked on No, logout should still be displayed - loggedIn" + loggedIn);
                    return;
                }
                else
                {
                    loggedIn = false;
                    //login.Text = "Login";
                    //event_label.Text = String.Format("Logged out");
                    System.Diagnostics.Debug.WriteLine("User clicked on Yes, login should be displayed - loggedIn" + loggedIn);
                    return;
                }
            }

        }

    }
}

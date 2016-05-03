using Acr.UserDialogs;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.Util;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    public class LoginPage : ContentPage
    {
        Entry email;
        Entry password;
        Button Login;

        private double longitude;
        private double latitude;

        public LoginPage()
        {
            Title = "Account";
            // BindingContext = new LoginViewModel(Navigation);
            //sets the background color based on settings
            if (Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;
            }

            Login = new Button { Text = "Login", TextColor = Color.White };

            //if (Helpers.Settings.LoginSetting == true)
            //{
            //    loggedIn = true;
            //    Login.Text = "Logout";
            //}
            //else
            //{
            //    loggedIn = false;
            //    Login.Text = "Login";
            //}
            Login.Clicked += Login_Clicked;

            var layout = new StackLayout { Padding = 10 };

            var layout2 = new StackLayout
            {
                BackgroundColor = Color.Gray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            email = new Entry { Placeholder = "Email", BackgroundColor = Color.White, PlaceholderColor = Color.Gray, TextColor = Color.Black };
            layout.Children.Add(email);

            password = new Entry { Placeholder = "Password", IsPassword = true, BackgroundColor = Color.White, PlaceholderColor = Color.Gray, TextColor = Color.Black };
            layout.Children.Add(password);

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

            Button signUpButton = new Button { Text = "Create an Account", TextColor = Color.White };
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
                Message = "Enter E-Mail",
                Placeholder = "youremail@example.com",
                InputType = InputType.Email
            });
            var stat = s.Ok ? "Success" : "Cancelled";
            // System.Diagnostics.Debug.WriteLine(stat, s);
           
            if (stat == "Success")
            {
                System.Diagnostics.Debug.WriteLine(stat + s.Text);
                //if (!(Regex.Match(s.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success))
                //{
                //    UserDialogs.Instance.ShowError("Invalid Email. Please try again.");
                //}
                //else
                //{
                    try
                    {
                        UserItem user = new UserItem { Item = new UserLogin { Email = s.Text.TrimEnd(), Arn = "" + Helpers.Settings.EndpointArnSetting } };
                        MessageJson messageJson = new MessageJson { operation = "forgotPassword", tableName = "User", payload = user };
                        string args = JsonConvert.SerializeObject(messageJson);

                        //System.Diagnostics.Debug.WriteLine(args);
                        var ir = new InvokeRequest()
                        {
                            FunctionName = "arn:aws:lambda:us-east-1:987221224788:function:Test_Backend",
                            PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args),
                            InvocationType = InvocationType.RequestResponse
                        };
                        //System.Diagnostics.Debug.WriteLine("Before invoke: " + ir.ToString());

                        InvokeResponse resp = await AmazonUtils.LambdaClient.InvokeAsync(ir);
                        resp.Payload.Position = 0;
                        var sr = new StreamReader(resp.Payload);
                        var myStr = sr.ReadToEnd();

                        System.Diagnostics.Debug.WriteLine("Status code: " + resp.StatusCode);
                        System.Diagnostics.Debug.WriteLine("Response content: " + myStr);

                        response tmp = JsonConvert.DeserializeObject<response>(myStr);
                        //System.Diagnostics.Debug.WriteLine("Success: " + tmp.Success);

                        if (tmp.Success == "true")
                        {
                            UserDialogs.Instance.ShowSuccess("Your password has been reset. Please check your email.");

                        }
                        else
                        {
                            UserDialogs.Instance.ShowError("Incorrect email. Please try again.");
                        }

                    //                System.Diagnostics.Debug.WriteLine("Status code: " + resp.StatusCode);
                    //                System.Diagnostics.Debug.WriteLine("Response content: " + myStr);
                }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("Error:" + e);
                    }

            }
        }

        private void SignUpButton_Clicked(object sender, EventArgs e)
        {
            // login_check();
            Navigation.PushAsync(new CreateUserPage());
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {

            UserDialogs.Instance.SuccessToast("Logging in..");

            //loggedIn = true;
            PushUser();

        }

        async private Task PushUser() //Function to create a Json object and send to server using a lambda function
        {
            //if (!(Regex.Match(email.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success))
            //{
            //    UserDialogs.Instance.ShowError("Invalid Email. Please try again.");
            //}
            //else
            //{
                getLocation();
                try
                {
                    UserItem user = new UserItem { Item = new UserLogin { Latitude = latitude, Longitude = longitude, Password = password.Text, Email = email.Text, Arn = "" + Helpers.Settings.EndpointArnSetting } };
                    MessageJson messageJson = new MessageJson { operation = "update", tableName = "User", payload = user };
                    string args = JsonConvert.SerializeObject(messageJson);
                    //System.Diagnostics.Debug.WriteLine(args);
                    var ir = new InvokeRequest()
                    {
                        FunctionName = "arn:aws:lambda:us-east-1:987221224788:function:Test_Backend",
                        PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args),
                        InvocationType = InvocationType.RequestResponse
                    };
                    //System.Diagnostics.Debug.WriteLine("Before invoke: " + ir.ToString());


                    InvokeResponse resp = await AmazonUtils.LambdaClient.InvokeAsync(ir);
                    resp.Payload.Position = 0;
                    var sr = new StreamReader(resp.Payload);
                    var myStr = sr.ReadToEnd();

                    System.Diagnostics.Debug.WriteLine("Status code: " + resp.StatusCode);
                    System.Diagnostics.Debug.WriteLine("Response content: " + myStr);

                    response tmp = JsonConvert.DeserializeObject<response>(myStr);
                    //System.Diagnostics.Debug.WriteLine("Success: " + tmp.Success);

                    if (tmp.Success == "true")
                    {
                        Helpers.Settings.LoginSetting = true;
                        ((RootPage)App.Current.MainPage).NavigateTo();
                        if (tmp.ActualUserType == "Volunteer")
                        {
                            Helpers.Settings.IsVolunteer = true;
                        }
                        Helpers.Settings.EmailSetting = email.Text;
                        Helpers.Settings.UserIDSetting = tmp.ID;
                        Helpers.Settings.UsernameSetting = tmp.OldNickname;
                    }
                    else
                    {
                        UserDialogs.Instance.ShowError("Incorrect credentials. Please try again.");
                    }

                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Error:" + e);
                }


            //}
        }

        async private Task getLocation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                locator.PositionChanged += OnPositionChanged;
                var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                latitude = Convert.ToDouble(position.Latitude);
                longitude = Convert.ToDouble(position.Longitude);

                //label2.Text = String.Format("Longitude: {0} Latitude: {1}", longtitude, latitude);
                System.Diagnostics.Debug.WriteLine("type of long/lat is: " + latitude.GetType());
                //await DisplayAlert("type", "" + latitude.GetType(), "ok");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                //label2.Text = "Unable to find location";
                latitude = 0.00;
                longitude = 0.00;
            }

        }

        async private void OnPositionChanged(object sender, PositionEventArgs e)
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
            var latitude = position.Latitude.ToString();
            var longtitude = position.Longitude.ToString();
            //label2.Text = String.Format("Longitude: {0} Latitude: {1}", longtitude, latitude);
        }

        //==================================================== Back Button Pressed ==============================================================

        protected override Boolean OnBackButtonPressed() // back button pressed
        {
            ((RootPage)App.Current.MainPage).NavigateTo();
            return true;
        }


    }//end of LoginPage class

    public class response{
        public string CognitoID { get; set; }
        public string Arn { get; set; }
        public string Success { get; set; }
        public string ActualUserType { get; set; }
        public string ID { get; set; }
        public string OldNickname { get; set; }

    }

}

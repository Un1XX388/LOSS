using Acr.UserDialogs;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.Util;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace LOSSPortable
{
    public class CreateUserPage : ContentPage
    {
        Entry email;
        Entry nickname;
        Button CreateAccount;
        String usertype;

        private double longitude;
        private double latitude;

        public CreateUserPage()
        {
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

            var layout = new StackLayout { Padding = 10 };

            var layout2 = new StackLayout
            {
                BackgroundColor = Color.Gray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            email = new Entry { Placeholder = "Email", BackgroundColor = Color.White, PlaceholderColor = Color.Gray, TextColor = Color.Black };
            layout.Children.Add(email);

            nickname = new Entry { Placeholder = "Username", BackgroundColor = Color.White, PlaceholderColor = Color.Gray, TextColor = Color.Black };
            layout.Children.Add(nickname);

            CheckBox isVolunteer = new CheckBox
            {
                DefaultText = "Sign up to be a volunteer for helping suicide survivors.",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Default, typeof(CheckBox)),
                Checked = false
            };
            isVolunteer.CheckedChanged += IsVolunteer_CheckedChanged;
            layout.Children.Add(isVolunteer);

            CreateAccount = new Button { Text = "Create Account", TextColor = Color.White };
            CreateAccount.Clicked += CreateAccount_Clicked;          
            layout.Children.Add(CreateAccount);


            //layout.Children.Add(layout2);
            this.Content = new ScrollView
            {
                Content = layout
            };

        }

        async private void CreateAccount_Clicked(object sender, System.EventArgs e)
        {
            await getLocation();
            if(nickname.Text == null || email.Text == null)
            {
                UserDialogs.Instance.ShowError("Please enter information in all fields.");
            }
            else
            {
                try
                {
                    UserItem user = new UserItem { Item = new UserLogin { Nickname = nickname.Text, Email = email.Text, Longitude = longitude, Latitude = latitude, UserType = usertype } };
                    MessageJson messageJson = new MessageJson { operation = "create", tableName = "User", payload = user };
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

                    UserDialogs.Instance.SuccessToast("Thank you for signing up!", "Please check your email for verification.", 3000 );

                    //                System.Diagnostics.Debug.WriteLine("Status code: " + resp.StatusCode);
                    //                System.Diagnostics.Debug.WriteLine("Response content: " + myStr);
                }
                catch (Exception e2)
                {
                    System.Diagnostics.Debug.WriteLine("Error:" + e2);
                }

                Helpers.Settings.EmailSetting = email.Text;
                ((RootPage)App.Current.MainPage).NavigateTo();

            }

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

        private void IsVolunteer_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            System.Diagnostics.Debug.WriteLine(e.Value);
            if(e.Value == true)
            {
                usertype = "Volunteer";
               
            }
            else
            {
                usertype = "Basic";
            };
        }
    }
}
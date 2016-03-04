using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.Util;

namespace LOSSPortable
{
    public class HomePage : ContentPage
    {
        Label label1 = new Label();
        Label label2 = new Label();
        Frame labelFrame;
        CancellationTokenSource cts;

        public HomePage()
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

            Debug.WriteLine("Hello world!");
            Title = "Home";
            label2.Text = "";
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
                OutlineColor = Color.FromHex("5A3A5C"),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center
            };

            Content = new StackLayout
            {
              //  Style = (Style)Application.Current.Resources["key"],
                Padding = new Thickness(30, Device.OnPlatform(20, 0, 0), 30, 30),
                Children = {
                    new ContentView {
                        Content = labelFrame
                    }
                },
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
        }

        async private void PushMessage()
        {
            try
            {

                //var lambdaConfig = new AmazonLambdaConfig { RegionEndpoint = Constants.COGNITO_REGION };
                var lambdaClient = new AmazonLambdaClient(AmazonUtils.Credentials, Constants.COGNITO_REGION);
                ChatMessage message = new ChatMessage { ToFrom = "Matthew#Isha", Time = "2016-02-29 11:27:13:22", Text = "Wow, this actually works!" };
                var args = @"{""operation"":""create"",""tableName"":""Message"",""Payload"":{""Item"":{""To#From"":""William"",""Time"":""2016-03-04 13:18:22:47"",""Text"":""William phone is silly!""}}}";
                //var args = "";
                //var context = AmazonUtils.DDBContext;
                System.Diagnostics.Debug.WriteLine("args: " + args);

                var ir = new InvokeRequest(){
                    FunctionName = "arn:aws:lambda:us-east-1:987221224788:function:Test_Backend",
                    //Payload = args,
                    PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args),
                    InvocationType = InvocationType.RequestResponse
                };
                System.Diagnostics.Debug.WriteLine("Before invoke: " + ir.ToString());


                InvokeResponse resp = await lambdaClient.InvokeAsync(ir);
                resp.Payload.Position = 0;
                var sr = new StreamReader(resp.Payload);
                var myStr = sr.ReadToEnd();

                System.Diagnostics.Debug.WriteLine("Status code: " + resp.StatusCode);
                System.Diagnostics.Debug.WriteLine("Response content: " + myStr);
                //context.SaveAsync(message);
                //AmazonLambdaRequest request = new AmazonLambdaRequest()
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error:" + e);
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
                var latitude = position.Latitude.ToString();
                var longtitude = position.Longitude.ToString();
                label2.Text = String.Format("Longitude: {0} Latitude: {1}", longtitude, latitude);
            }
            catch (Exception ex)
            {
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            cts = new CancellationTokenSource();
            LoadQuotes().ContinueWith(task => {
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        PushMessage();
                        label1.Text = task.Result.Message;
                        labelFrame.OutlineColor = Color.White;
                    }
                    catch (Exception e)
                    {

                    }
                });
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (cts != null)
            {
                cts.Cancel();
            }
        }


        private Task<InspirationalQuote> LoadQuotes()
        {
            var context = AmazonUtils.DDBContext;
            /*List<ScanCondition> conditions = new List<ScanCondition>();
            var SearchBar = context.ScanAsync<InspirationalQuote>(conditions);
            return SearchBar.GetNextSetAsync();*/
<<<<<<< HEAD
            int num = rnd.Next(1, 25);
=======
            
            int num = rnd.Next(1,25);
>>>>>>> 1c25b2906534bed115b4e7b3070a1b5a1ddc0201
            return context.LoadAsync<InspirationalQuote>(num.ToString(), cts.Token);

        }

        static Random rnd = new Random();
    }
}

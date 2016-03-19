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

            Title = "Home";
            label1.Text = LoadQuotes().Message;
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
                OutlineColor = Color.Transparent,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
            };
            if (label1.Text != "")
            {
                labelFrame.OutlineColor = Color.White;
            }

            var logo = new Image { Aspect = Aspect.AspectFit };
            logo.Source = Device.OnPlatform(
            iOS: ImageSource.FromFile("App6.png"),
            Android: ImageSource.FromFile("logo2.png"),
            WinPhone: ImageSource.FromFile("App6.png"));
            logo.VerticalOptions = LayoutOptions.StartAndExpand;
            logo.HeightRequest = 200;

            //909479
            var SOS_link = new Button
            {
                Text = "Survivors of Suicide Handbook",
                TextColor = Colors.barBackground,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.FromHex("ffffe6"),
                HeightRequest = 40,
                WidthRequest = 300
            };
            SOS_link.Clicked += SOSLinkPressed;

            var sg_link = new Button
            {
                Text = "Support Groups",
                TextColor = Colors.barBackground,
                HorizontalOptions = LayoutOptions.StartAndExpand,
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
                                // BackgroundColor = Color.FromHex("735974")
                                },

                            },
                VerticalOptions = LayoutOptions.EndAndExpand,
            };

            var mainContent = new StackLayout
            {
                //  Style = (Style)Application.Current.Resources["key"],
                Padding = new Thickness(30, Device.OnPlatform(20, 0, 0), 30, 30),
                Children = {
                    new BoxView() { Color = Color.Transparent, HeightRequest = 5  },
                    logo,
                    new ContentView {
                        Content = labelFrame
                    },
                    links
                },
                //  VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            ScrollView content = new ScrollView
            {
                Content = mainContent,
                Orientation = ScrollOrientation.Vertical
            };
            this.Content = content;

        }

        async private void PushMessage(string toFrom, string text)
        {
            try
            {
                MessageItem message = new MessageItem{Item = new ChatMessage { ToFrom = toFrom, Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff"), Text = text}};
                MessageJson messageJson = new MessageJson { operation = "create", tableName = "Message", payload = message };
                string args = JsonConvert.SerializeObject(messageJson);
                //System.Diagnostics.Debug.WriteLine(args);
                var ir = new InvokeRequest(){
                    FunctionName = "arn:aws:lambda:us-east-1:987221224788:function:Test_Backend",
                    PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args),
                    InvocationType = InvocationType.RequestResponse
                };
                //System.Diagnostics.Debug.WriteLine("Before invoke: " + ir.ToString());


                InvokeResponse resp = await AmazonUtils.LambdaClient.InvokeAsync(ir);
                resp.Payload.Position = 0;
                var sr = new StreamReader(resp.Payload);
                var myStr = sr.ReadToEnd();

//                System.Diagnostics.Debug.WriteLine("Status code: " + resp.StatusCode);
//                System.Diagnostics.Debug.WriteLine("Response content: " + myStr);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error:" + e);
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        
        

        async void SOSLinkPressed(object sender, EventArgs e)
        {
            WebView webview = new WebView();
            //http://stackoverflow.com/questions/2655972/how-can-i-display-a-pdf-document-into-a-webview
            //using google docs viewer
            String pdf = "http://www.suicidology.org/Portals/14/docs/Survivors/Loss%20Survivors/SOS_handbook.pdf";
            webview.Source = "http://drive.google.com/viewerng/viewer?embedded=true&url=" + pdf;

            //Device.OpenUri(new System.Uri("http://www.suicidology.org/Portals/14/docs/Survivors/Loss%20Survivors/SOS_handbook.pdf"));

            //WebView webView = new WebView
            //{
            //    Source = new UrlWebViewSource
            //    {
            //        Url = "http://www.suicidology.org/Portals/14/docs/Survivors/Loss%20Survivors/SOS_handbook.pdf"
            //    },
            //    VerticalOptions = LayoutOptions.FillAndExpand
            //};
            await Navigation.PushAsync(new ContentPage()
            {
                Content = webview

            });
        }


        async void sgLinkPressed(object sender, EventArgs e)
        {

        //    sg_link.Enabled = false;

            WebView webView = new WebView
            {
                Source = new UrlWebViewSource
                {
                    Url = "http://afsp.org/find-support/ive-lost-someone/find-a-support-group/",
                },
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            await Navigation.PushAsync(new ContentPage()
            {
                Content = webView

            });
        //    sg_link.Enabled = true;
        }

        

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
            System.Diagnostics.Debug.WriteLine("Size : " + size + "\n num :" + num);
            return quoteList[num];
        }

        void quoteList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            label1.Text = LoadQuotes().Message;
            labelFrame.OutlineColor = Color.White;
        }


        


        
    }

}

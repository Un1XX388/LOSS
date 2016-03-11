﻿using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.IO;
using System.Diagnostics;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.Util;
using Newtonsoft.Json;

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
                HorizontalOptions = LayoutOptions.Center,
            };


            var logo = new Image { Aspect = Aspect.AspectFit };
            logo.Source = Device.OnPlatform(
            iOS: ImageSource.FromFile("App6.png"),
            Android: ImageSource.FromFile("logo2.png"),
            WinPhone: ImageSource.FromFile("App6.png"));
            logo.VerticalOptions = LayoutOptions.StartAndExpand;
            logo.HeightRequest = 200;

            var SOS_link = new Button
            {
                Text = "Survivors of Suicide Handbook",
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.Transparent,
                HeightRequest = 40
            };
            SOS_link.Clicked += SOSLinkPressed;

            var sg_link = new Button
            {
                Text = "Support Groups",
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.Transparent,
                HeightRequest = 40
            };
            sg_link.Clicked += sgLinkPressed;

            //var SOS_link = new StackLayout
            //{
            //    Children = { new Label {
                
            //        Text = "Survivors of Suicide Handbook",
            //        TextColor = Color.White,
            //        HorizontalOptions = LayoutOptions.StartAndExpand,
            //        BackgroundColor = Color.Transparent,
            //        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            //        }
            //    },
            //    GestureRecognizers = {

            //    new TapGestureRecognizer {
            //       //     Command = new Command (()=>System.Diagnostics.Debug.WriteLine ("clicked")),
            //            Command = new Command (()=> SOSLinkPressed()),
            //    },
            //    },
            //    Orientation = StackOrientation.Horizontal,
            //    Padding = new Thickness(5, 5)

            //};

            //StackLayout sg_link = new StackLayout
            //{
            //    Children = {
            //        new Label
            //        {
            //             Text = "Support Groups",
            //             TextColor = Color.White,
            //             HorizontalOptions = LayoutOptions.StartAndExpand,
            //             BackgroundColor = Color.Transparent,
            //             FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            //        }
            //    },
            //    GestureRecognizers = {

            //    new TapGestureRecognizer {
            //       //     Command = new Command (()=>System.Diagnostics.Debug.WriteLine ("clicked")),
            //            Command = new Command (()=>sgLinkPressed()),
            //    },
            //    },
            //    Orientation = StackOrientation.Horizontal,
            //    Padding = new Thickness(5, 5)
            //};

            StackLayout links = new StackLayout
            {
                Padding = new Thickness(0, Device.OnPlatform(30, 0, 0), 0, 0),
                Children = {
                              new BoxView() { Color = Color.Transparent, HeightRequest = 4  },
                                 new Label { Text = "Helpful Resources", TextColor = Color.White, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))},
                                 new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.5  },
                                 new BoxView() { Color = Color.Transparent, HeightRequest = 5  },
                                 SOS_link,
                                 new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.1  },
                                 new BoxView() { Color = Color.Transparent, HeightRequest = 5  },
                                 sg_link,

                            },
                VerticalOptions = LayoutOptions.EndAndExpand
            };

            var mainContent = new StackLayout
            {
                //  Style = (Style)Application.Current.Resources["key"],
                Padding = new Thickness(30, Device.OnPlatform(20, 0, 0), 30, 30),
                Children = {
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

            //Content = new StackLayout
            //{
            //    //  Style = (Style)Application.Current.Resources["key"],
            //    Padding = new Thickness(30, Device.OnPlatform(20, 0, 0), 30, 30),
            //    Children = {
            //        logo,
            //        new ContentView {
            //            Content = labelFrame
            //        }
            //    },
            //    //  VerticalOptions = LayoutOptions.CenterAndExpand,
            //    HorizontalOptions = LayoutOptions.CenterAndExpand,
            //};

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
                        label1.Text = task.Result.Message;
                        labelFrame.OutlineColor = Color.White;
                        //first parameter is the toFrom field, second is the Text field.
                        PushMessage("tester" + rnd.Next().ToString(), "testing string, please ignore!");
                     
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

            int num = rnd.Next(1, 25);
            
            return context.LoadAsync<InspirationalQuote>(num.ToString(), cts.Token);

        }

        static Random rnd = new Random();

        void SOSLinkPressed(object sender, EventArgs e)
        {
            WebView webView = new WebView
            {
                Source = new UrlWebViewSource
                {
                    Url = "http://www.suicidology.org/Portals/14/docs/Survivors/Loss%20Survivors/SOS_handbook.pdf"

                },
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            Navigation.PushAsync(new ContentPage()
            {
                Content = webView

            });
        }


        void sgLinkPressed(object sender, EventArgs e)
        {


            WebView webView = new WebView
            {
                Source = new UrlWebViewSource
                {
                    Url = "http://afsp.org/find-support/ive-lost-someone/find-a-support-group/",
                },
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            Navigation.PushAsync(new ContentPage()
            {
                Content = webView

            });

        }
    }

}

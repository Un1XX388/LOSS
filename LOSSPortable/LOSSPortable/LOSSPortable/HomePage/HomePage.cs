using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;

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
            
            BackgroundColor = Color.White;
            
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

            labelFrame = new Frame {Content =  label1,
                            OutlineColor = Color.FromHex("5A3A5C"),
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.Center
                        };

            Content = new StackLayout
            {
                Style = (Style)Application.Current.Resources["key"],
                //Padding = new Thickness(30, Device.OnPlatform(20, 0, 0), 30, 30),
                Children = {
                    new ContentView {
                        Content = labelFrame
                    }
                },
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
        }

        async private Task getLocation(){
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                locator.PositionChanged += OnPositionChanged;
                var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                var latitude = position.Latitude.ToString();
                var longtitude =position.Longitude.ToString();
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
            LoadQuotes().ContinueWith(task =>{
                Device.BeginInvokeOnMainThread(() =>
                    {
                        try {
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
            int num = rnd.Next(1, 5);
            return context.LoadAsync<InspirationalQuote>(num.ToString(), cts.Token);
            
         }

        static Random rnd = new Random();
        
        /*private Task showQuoteOfDay()
        {
            //quotesList = await test.GetTaskQuoteAsync();
          
            //DisplayAlert("list size: ",""+ quotesList.Count,"OK");
            
            //int i = rnd.Next(quotesList.Count-1);
            //QuoteOfDay = quotesList[i];
            //System.Diagnostics.Debug.WriteLine(QuoteOfDay.ID, QuoteOfDay.inspirationalQuote);
            //label1.Text = String.Format(QuoteOfDay.inspirationalQuote);
            label1.Text = "hello world";
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
        }*/

    }
}

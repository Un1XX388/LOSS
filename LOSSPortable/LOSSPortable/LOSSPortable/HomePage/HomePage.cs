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
using Xamarin.Forms;

namespace LOSSPortable
{
    public class HomePage : ContentPage
    {
        Label label2 = new Label();
        Label label1 = new Label();

        Quote QuoteOfDay = new Quote();

        List<Quote> quotesList;
        ListView listView;


        public HomePage()
        {
            QuoteOfDay = new Quote();
            label2.Text = "Loading...";
            listView = new ListView
            {
                RowHeight = 40,
                ItemTemplate = new DataTemplate(typeof(testItemCell))
            };
            Content = new StackLayout
            {
                Children = {
                    listView
                }
            };
            /*Content = new StackLayout
            {
                //Children = {
                //    label1
                //    //new Label { Text = "This is the home page!" }
                //}
                Padding = new Thickness(30, Device.OnPlatform(20, 0, 0), 30, 30),

                Children = {
                    new ContentView {
         
                        Content = new Frame {Content =  label1,
                        OutlineColor = Color.White,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.Center
                    },

                     VerticalOptions = LayoutOptions.CenterAndExpand,
                     HorizontalOptions = LayoutOptions.Center


                    }

                }

            };
             */
            //getLocation();
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
            LoadQuotes().ContinueWith(task =>{
                Device.BeginInvokeOnMainThread(() =>
                    {
                        listView.ItemsSource = task.Result;
                    });
            });
            /*AmazonUtils.transferUtility.DownloadAsync(Path.Combine(Environment.SpecialFolder.ApplicationData, "file"),
                "bucketName",
                "key");*/
        }


        private Task<List<InspirationalQuote>> LoadQuotes()
        {
            var context = AmazonUtils.DDBContext;
            List<ScanCondition> conditions = new List<ScanCondition>();
            var SearchBar = context.ScanAsync<InspirationalQuote>(conditions);
            return SearchBar.GetNextSetAsync();
        }

        //static Random rnd = new Random();
        
        /*private Task showQuoteOfDay()
        {
            //quotesList = await test.GetTaskQuoteAsync();
          
            //DisplayAlert("list size: ",""+ quotesList.Count,"OK");
            
            //int i = rnd.Next(quotesList.Count-1);
            //QuoteOfDay = quotesList[i];
            //System.Diagnostics.Debug.WriteLine(QuoteOfDay.ID, QuoteOfDay.inspirationalQuote);
            //label1.Text = String.Format(QuoteOfDay.inspirationalQuote);
            label1.Text = testItems[0].ToString();
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

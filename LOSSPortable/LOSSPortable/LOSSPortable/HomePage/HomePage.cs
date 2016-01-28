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

        public HomePage()
        {
            QuoteOfDay = new Quote();
            label2.Text = "Loading...";

            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "This is the home page!" },
                    label2, label1
                }
               
            };
            getLocation();
            showQuoteOfDay();
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

        static Random rnd = new Random();
        async private Task showQuoteOfDay()
        {
            quotesList = await App.PManager.GetTaskQuoteAsync();
         //   DisplayAlert("list size: ",""+ quotesList.Count,"OK");

            int i = rnd.Next(quotesList.Count-1);
            QuoteOfDay = quotesList[i];
            System.Diagnostics.Debug.WriteLine(QuoteOfDay.ID, QuoteOfDay.inspirationalQuote);
            label1.Text = String.Format(QuoteOfDay.ID + "   " + QuoteOfDay.inspirationalQuote);

        }

    }
}

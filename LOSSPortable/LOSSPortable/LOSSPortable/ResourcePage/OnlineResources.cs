using System;
using Xamarin.Forms;
using System.Collections.Generic;
using Plugin.TextToSpeech;

namespace LOSSPortable{
    public class OnlineResources : ContentPage {
        public OnlineResources() {
            //sets the background color based on settings
            if (Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;
            }

            Title = "Online Resources";

            WebView webView = new WebView
            {
                Source = new UrlWebViewSource
                {
                    Url = "http://www.suicidology.org/Portals/14/docs/Survivors/Loss%20Survivors/25-Suggestions-For-Survivors.pdf",
                },
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            this.Content = webView;
            CrossTextToSpeech.Current.Speak(Content.ToString());

        }// End of OnlineResources() constructor.n


        //private async void starthttp()
        //{
        //    WebRequest request = HttpWebRequest.Create("http://blog.xamarin.com");

        //    WebResponse response = await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null);

        //    var html = new HtmlDocument();
        //    html.Load(response.GetResponseStream());
        //    var nodes = html.DocumentNode.Descendants();
        //    // The root page of your application
        //    string result = "";
        //    foreach (var node in nodes)
        //    {

        //        result = result + node.OuterHtml;

        //    }
        //    MainPage = new ContentPage
        //    {
        //        Content = new StackLayout
        //        {
        //            VerticalOptions = LayoutOptions.Center,
        //            Children = {
        //            new Label {

        //                Text = result
        //            }
        //        }
        //        }
        //    };
        //}

    }// End of OnlineResources class.
}// End of namespace LOSS.
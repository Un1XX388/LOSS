using Plugin.TextToSpeech;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    public class OnlineResources : ContentPage
    {

        // Holds all info for each item on resources page.
        public ObservableCollection<OnlineRViewModel> online_resources { get; set; }

        // 
        public OnlineResources()
        {
            //sets the background color based on settings
            if (Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else {
                BackgroundColor = Colors.background;
            }

            // Holds data to be displayed on this content page.
            online_resources = new ObservableCollection<OnlineRViewModel>();

            // View type for this content page.
            ListView lstView = new ListView();

            // Set size (height) of each element displayed on this page.
            lstView.RowHeight = 70;

            // Set title of this page.

            Title = "Online Resources";
            // Set source of data for the list view used on this page.
            lstView.ItemsSource = online_resources;

            // Set layout for each element in this list view.
            lstView.ItemTemplate = new DataTemplate(typeof(OnlineResourceCell));

            // Set behavior of element when selected by user.
            lstView.ItemSelected += Onselected;

            // Assign the list view created above to this content page.
            Content = lstView;

            //populate listview with retrieved online resources from the server
            var tempList = LoadResources();
            for(int i = 0; i < tempList.Count; i++)
            {

                online_resources.Add(new OnlineRViewModel
                {
                    Image       = "quote64.png",
                    Title       = tempList[i].Title,
                    Description = tempList[i].Description,
                    URL         = tempList[i].URL,
                    Type        = tempList[i].Type,
                    Fav         = "Fav"

                });

                            
            }

            // Accomodate iPhone status bar.
            Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
        }// End of OnlineResources() constructor.

        //loads list of online resources from server
        private RangeObservableCollection<OnlineRViewModel> LoadResources()
        {
            RangeObservableCollection<OnlineRViewModel> resources = AmazonUtils.getOnlineRList; ;
            return resources;
        }


        void Onselected(object sender, SelectedItemChangedEventArgs e)
        {

            if (e.SelectedItem == null)
            {
                return;
            }
            ((ListView)sender).SelectedItem = null;        // This deselects the item after it is selected.

            String title = e.SelectedItem.ToString().Split(',')[1];

            //checks if the item type is pdf or website
            if (e.SelectedItem.ToString().Split(',')[4].Equals("PDF"))
            {
                WebView webview = new WebView();
                //http://stackoverflow.com/questions/2655972/how-can-i-display-a-pdf-document-into-a-webview
                //using google docs viewer
                String pdf = e.SelectedItem.ToString().Split(',')[3];
                webview.Source = "http://drive.google.com/viewerng/viewer?embedded=true&url=" + pdf;

                Navigation.PushAsync(new ContentPage()
                {
                    Title = title,
                    Content = webview
                });
            }
            else
            {

                WebView webView = new WebView
                {
                    Source = new UrlWebViewSource
                    {
                        Url = e.SelectedItem.ToString().Split(',')[3],
                    },
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                Navigation.PushAsync(new ContentPage()
                {
                    Title = title,
                    Content = webView

                });
            }
        }// End of Onselected() method.

        //navigate to homepage when back button pressed
        protected override Boolean OnBackButtonPressed()
        {
            ((RootPage)App.Current.MainPage).NavigateTo();
            return true;
        }// End of OnBackButtonPressed() method.

    }
}

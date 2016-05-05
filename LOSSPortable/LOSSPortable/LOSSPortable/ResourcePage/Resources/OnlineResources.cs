using Plugin.TextToSpeech;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace LOSSPortable
{
    public class OnlineResources : ContentPage
    {

        // Holds all info for each item on resources page.
        public RangeObservableCollection<OnlineRViewModel> online_resources { get; set; }
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


            OnlineResourceCell temp = new OnlineResourceCell();


            // Holds data to be displayed on this content page.
            online_resources = AmazonUtils.getOnlineRList;

            //Switch case for differnet icons depending on file type
            for (int i = 0; i < online_resources.Count; i++)
            {
                switch (online_resources[i].Type)
                {
                    case "Website":
                        online_resources[i].Image = "webdesign.png";
                        break;
                    case "PDF":
                        online_resources[i].Image = "pdf.png";
                        break;
                    default:
                        online_resources[i].Image = "webdesign.png";
                        break;
                }
            }

            // View type for this content page.
            ListView lstView = new ListView();

            // Set size (height) of each element displayed on this page.
            lstView.RowHeight = 70;

            // Set title of this page.

            Title = "Online Resources";
            // Set source of data for the list view used on this page.
            lstView.ItemsSource = online_resources;

            //var cellFav = new Image();
            //cellFav.VerticalOptions = LayoutOptions.Center;
            //cellFav.HorizontalOptions = LayoutOptions.Center;
            //cellFav.SetBinding(Image.SourceProperty, new Binding("Fav"));
            //temp.cellView.Children.Add(cellFav, 4, 5, 0, 13);


            // Set layout for each element in this list view.
            lstView.ItemTemplate = new DataTemplate(typeof(OnlineResourceCell));

            lstView.BackgroundColor = BackgroundColor;
            // Set behavior of element when selected by user.
            lstView.ItemSelected += Onselected;

            // Assign the list view created above to this content page.
            Content = lstView;
            
            //populate listview with retrieved online resources from the server
            
            // Accomodate iPhone status bar.
            Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

        }// End of OnlineResources() constructor.



        //public String isFavorited()
        //{
        //    if (favClicked == false)
        //    {
        //        Helpers.Settings.FavoriteSetting = false;
        //        favClicked = true;
        //        return "fav132.png";
        //    }
        //    else
        //    {
        //        //get all the items favorited for caching
        //        Helpers.Settings.FavoriteSetting = true;
        //        favClicked = false;
        //        return "fav232.png";
        //    }
        //}
        //loads list of online resources from server
        private RangeObservableCollection<OnlineRViewModel> LoadResources()
        {
            RangeObservableCollection<OnlineRViewModel> resources = AmazonUtils.getOnlineRList;
   
            //for (int k = 0; k < resources.Count; k++)
            //{
            //    resources[k].Fav = "fav132.png";
            //}
            return resources;
        }

        
        void Onselected(object sender, SelectedItemChangedEventArgs e)
        {

            if (e.SelectedItem == null)
            {
                return;
            }
            ((ListView)sender).SelectedItem = null;        // This deselects the item after it is selected.
            var select = e.SelectedItem as OnlineRViewModel;
            String title = select.Title;
            String desc = select.Description;
            String link = select.URL;
            if(Helpers.Settings.SpeechSetting == true)
            {
                CrossTextToSpeech.Current.Speak(title);
            }
			UserDialogs.Instance.ShowLoading();

        //    int count = Convert.ToInt32(e.SelectedItem.ToString().Split(',')[5]);

			WebView webview = new WebView();

            //checks if the item type is pdf or website
            if (select.Type == "PDF")
            {
                //http://stackoverflow.com/questions/2655972/how-can-i-display-a-pdf-document-into-a-webview
                //using google docs viewer
                webview.Source = "http://drive.google.com/viewerng/viewer?embedded=true&url=" + link;
                webview.VerticalOptions = LayoutOptions.FillAndExpand;
                webview.HorizontalOptions = LayoutOptions.FillAndExpand;

                Navigation.PushAsync(new ContentPage()
                {
                    Title = title,
                    Content = webview
                });
            }
            else
			{
				webview.Source = link;
				webview.VerticalOptions = LayoutOptions.FillAndExpand;
				webview.HorizontalOptions = LayoutOptions.FillAndExpand;

                Navigation.PushAsync(new ContentPage()
                {
                    Title = title,
                    Content = webview
                });
							
            }

			UserDialogs.Instance.HideLoading();

        }// End of Onselected() method.

        protected override void OnDisappearing()
        {
            CrossTextToSpeech.Dispose();

            base.OnDisappearing();

        }
        //navigate to homepage when back button pressed
        protected override Boolean OnBackButtonPressed()
        {
            ((RootPage)App.Current.MainPage).NavigateTo();
            return true;
        }// End of OnBackButtonPressed() method.

    }
}

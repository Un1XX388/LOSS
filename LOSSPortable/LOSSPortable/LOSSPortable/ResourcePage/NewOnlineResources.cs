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
    public class NewOnlineResources : ContentPage
    {

        // Holds all info for each item on resources page.
        public ObservableCollection<OnlineRViewModel> online_resources { get; set; }

        // 
        public NewOnlineResources()
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
                    Title       = tempList[i].Title.ToString(),
                    Description = tempList[i].Description,
                    URL         = tempList[i].URL,
                    Type        = tempList[i].Type,
                    Fav         = "Fav"
                });       
                

                            
            }

            //for(int i=0; i < online_resources.Count; i++)
            //{
            //    System.Diagnostics.Debug.WriteLine("ONLINE RESOURCE: " + online_resources[i].Title);
            //}

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

            var item = e.SelectedItem;
            

        }// End of Onselected() method.

        //navigate to homepage when back button pressed
        protected override Boolean OnBackButtonPressed()
        {
            ((RootPage)App.Current.MainPage).NavigateTo();
            return true;
        }// End of OnBackButtonPressed() method.

    }
}

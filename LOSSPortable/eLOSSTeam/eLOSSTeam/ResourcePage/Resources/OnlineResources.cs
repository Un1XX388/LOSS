﻿using Plugin.TextToSpeech;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace eLOSSTeam
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
            online_resources = AmazonUtils.getOnlineRList;            // Holds data to be displayed on this content page.

            //Switch case for differnet icons depending on file type
            for (int i = 0; i < online_resources.Count; i++)
            {
				
				if (online_resources [i].Description.Length > 110) {
					online_resources [i].Description = online_resources [i].Description.Substring (0, 110) + "...";
				}
				
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

            ListView lstView = new ListView();

            // Set size (height) of each element displayed on this page.
            lstView.RowHeight = 100;
            Title = "Online Resources";
            lstView.ItemsSource = online_resources;                         // Set source of data for the list view used on this page.
            lstView.ItemTemplate = new DataTemplate(typeof(OnlineResourceCell)); // Set layout for each element in this list view.
            lstView.BackgroundColor = BackgroundColor;
            lstView.ItemSelected += Onselected;                                 // Set behavior of element when selected by user.


            // Assign the list view created above to this content page.
            Content = lstView;
                        
            // Accomodate iPhone status bar.
            Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

        }// End of OnlineResources() constructor.


        //Displays item in webview depending on its type      
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


        //stop speech when page disappears
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

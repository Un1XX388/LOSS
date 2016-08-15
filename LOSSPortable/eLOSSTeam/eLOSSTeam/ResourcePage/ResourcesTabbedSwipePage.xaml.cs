using Plugin.TextToSpeech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace eLOSSTeam
{
    public partial class ResourcesTabbedSwipePage : TabbedPage
    {
        public ResourcesTabbedSwipePage()
        {
            Title = "Resources";

            //sets the background color based on settings
            if (Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;
            }

            InitializeComponent();

            var page1 = new OnlineResources();
            var page2 = new VideoResources();

            Web.Children.Add(page1);
            Web.Children.Add(new ContentPage() { BackgroundColor = setBackgroundColor() });
            Web.Children.Add(new ContentPage() { BackgroundColor = setBackgroundColor() });

            Playlist.Children.Add(new ContentPage() { BackgroundColor = setBackgroundColor() });
			//Playlist.Children.Add(new ContentPage() { BackgroundColor = setBackgroundColor() });
            Playlist.Children.Add(page2);

            AttachCurrentPageChanged();

        }

        public Color setBackgroundColor()
        {
            if(Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;
            }
            return BackgroundColor;
        }
        private void AttachCurrentPageChanged()
        {
            Web.CurrentPageChanged += MultiPage_OnCurrentPageChanged;
            Playlist.CurrentPageChanged += MultiPage_OnCurrentPageChanged;
//            Favorites.CurrentPageChanged += MultiPage_OnCurrentPageChanged;
            this.CurrentPageChanged += MultiPage1_OnCurrentPageChanged;

            System.Diagnostics.Debug.WriteLine("In AttachCurrentPageChanged function!");

        }

        private void DetachCurrentPageChanged()
        {
            //  CurrentPageChanged -= MultiPage_OnCurrentPageChanged;
            Web.CurrentPageChanged -= MultiPage_OnCurrentPageChanged;
            Playlist.CurrentPageChanged -= MultiPage_OnCurrentPageChanged;
 //           Favorites.CurrentPageChanged -= MultiPage_OnCurrentPageChanged;
            this.CurrentPageChanged -= MultiPage1_OnCurrentPageChanged;

        }


        private void MultiPage1_OnCurrentPageChanged(object sender, EventArgs e)
        {
            TabbedPage tabbedPage = sender as TabbedPage;
            if (tabbedPage != null)
            {
                CarouselPage currentPage = tabbedPage.CurrentPage as CarouselPage;
                currentPage.CurrentPage = currentPage.Children[tabbedPage.Children.IndexOf(tabbedPage.CurrentPage)];
            }
        }

        private void MultiPage_OnCurrentPageChanged(object sender, EventArgs e)
        {
            DetachCurrentPageChanged();

            System.Diagnostics.Debug.WriteLine("In MultiPage function!");

            CarouselPage carouselPage = sender as CarouselPage;
            if (carouselPage != null)
            {
                System.Diagnostics.Debug.WriteLine("In MultiPage if statement!");

                int indexOf = carouselPage.Children.IndexOf(carouselPage.CurrentPage);

                var tabbedPage = carouselPage.ParentView as TabbedPage;

                if (tabbedPage != null)
                {
                    tabbedPage.CurrentPage = tabbedPage.Children[indexOf];

                    var newCarouselPage = tabbedPage.CurrentPage as CarouselPage;

                    if (newCarouselPage != null)
                    {
                        newCarouselPage.CurrentPage = newCarouselPage.Children[indexOf];
                    }
                }
            }
            AttachCurrentPageChanged();
        }

        protected override Boolean OnBackButtonPressed()
        {
            ((RootPage)App.Current.MainPage).NavigateTo();
            return true;
        }// End of OnBackButtonPressed() method.
    }

}


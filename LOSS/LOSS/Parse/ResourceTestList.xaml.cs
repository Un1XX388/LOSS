using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace LOSS
{
	public partial class ResourceTestList : ContentPage
	{
		public ResourceTestList ()
		{
			InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            listView.ItemsSource = await App.ResourceManager.GetTasksAsync();
        }

        void OnAddItemActivated(object sender, EventArgs e)
        {
            var resourceItem = new ResourcesPage();
            var resourcePage = new ResourceTest();
            resourcePage.BindingContext = resourceItem;
            Navigation.PushAsync(resourcePage);
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var resourceItem = e.SelectedItem as ResourceItem;
            var resourcePage = new ResourceTest();
            resourcePage.BindingContext = resourceItem;
            Navigation.PushAsync(resourcePage);
        }
	}
}

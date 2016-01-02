using System;
using Xamarin.Forms;

namespace LOSS
{
	public partial class ResourceTest : ContentPage
	{
		public ResourceTest ()
		{
			InitializeComponent();
		}

        async void OnSaveActivated(object sender, EventArgs e)
        {
            var todoItem = (ResourceItem)BindingContext;
            await App.ResourceManager.SaveTaskAsync(todoItem);
            await Navigation.PopAsync();
        }

        async void OnDeleteActivated(object sender, EventArgs e)
        {
            var todoItem = (ResourceItem)BindingContext;
            await App.ResourceManager.DeleteTaskAsync(todoItem);
            await Navigation.PopAsync();
        }

        void OnCancelActivated(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
	}
}

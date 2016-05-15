using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace eLOSSTeam{
	public class RootPage : MasterDetailPage{
		MasterPage masterPage;

		public RootPage(){
			masterPage					= new MasterPage();
			Master						= masterPage;
			Detail						= new NavigationPage(new HomePage()) { BarBackgroundColor = customNavBarColor(), BarTextColor = Color.White }; 
            masterPage.BackgroundColor	= Color.White;
            
			masterPage.ListView.ItemSelected += OnItemSelected;

		}// End of RootPage() method.

		void OnItemSelected(object sender, SelectedItemChangedEventArgs e){
			var item = e.SelectedItem as MasterPageItem;
			if (item != null){
                try
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType)) { BarBackgroundColor = customNavBarColor(), BarTextColor = Color.White };
                    masterPage.ListView.SelectedItem = null;
                    IsPresented = false;
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    System.Diagnostics.Debug.WriteLine("Start stack trace : " + ex);
                }
			}
		}// End OnItemSelected() method.

        //change navigation bar color based on contrast settings
        Color customNavBarColor()
        {
            if(Helpers.Settings.ContrastSetting == true)
            {
                return Color.Black;
            }
            else
            {
                return Colors.barBackground;
            }
        }

        //navigate back to a homepage instead of exiting the app
        public void NavigateTo()
        {
            this.Detail = new NavigationPage(new HomePage()) { BarBackgroundColor = customNavBarColor() };
            this.Title = "Home";
        }

    }// End of RootPage class.
}// End of LOSSPortable namespace.
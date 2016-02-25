using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace LOSSPortable{
	public class RootPage : MasterDetailPage{
		MasterPage masterPage;

		public RootPage(){
			masterPage					= new MasterPage();
			Master						= masterPage;
			Detail						= new NavigationPage(new HomePage()) { BarBackgroundColor = customNavBarColor()}; 
            masterPage.BackgroundColor	= Color.White;
            
			masterPage.ListView.ItemSelected += OnItemSelected;
		}// End of RootPage() method.

		void OnItemSelected(object sender, SelectedItemChangedEventArgs e){
			var item = e.SelectedItem as MasterPageItem;
			if (item != null){
				Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType)) { BarBackgroundColor = customNavBarColor()} ;
                this.Detail.BackgroundColor = Color.Pink;
                masterPage.ListView.SelectedItem = null;
				IsPresented = false;
			}
		}// End OnItemSelected() method.

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
	}// End of RootPage class.
}// End of LOSSPortable namespace.
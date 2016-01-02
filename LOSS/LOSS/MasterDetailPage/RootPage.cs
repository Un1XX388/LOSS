using System;
using Xamarin.Forms;
using System.Linq;

namespace LOSS{
	public class RootPage : MasterDetailPage{
		MenuPage menuPage;

		public RootPage(){
			menuPage					= new MenuPage ();
			menuPage.Menu.ItemSelected	+= (sender, e) => NavigateTo (e.SelectedItem as MenuItem);
			Master						= menuPage;
			Detail						= new NavigationPage(new ResourcesPage());
		}

		void NavigateTo(MenuItem menu){
			if (menu == null)
				return;
			
			Page displayPage			= (Page)Activator.CreateInstance(menu.TargetType);
			Detail						= new NavigationPage(displayPage);
			menuPage.Menu.SelectedItem	= null;
			IsPresented					= false;
		}// End of NavigateTo() method.
	}// End of RootPage class.
}// End of namespace LOSS.
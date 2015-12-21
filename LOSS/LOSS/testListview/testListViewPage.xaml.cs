using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Xamarin.Forms;

namespace LOSS
{
	public partial class testListViewPage : ContentPage
	{
        public ObservableCollection<ResourceViewModel> resources { get; set; }
		public testListViewPage ()
		{
            resources = new ObservableCollection<ResourceViewModel>();
			InitializeComponent ();
            lstView.ItemsSource = resources;
            resources.Add(new ResourceViewModel { title = "accounts", subtitle = "accounts_description", image = "accounts.png" });
            resources.Add(new ResourceViewModel { title = "contacts", subtitle = "contacts_description", image = "contacts.png" });
            resources.Add(new ResourceViewModel { title = "icon", subtitle = "icon_description", image = "Icon.png" });
            resources.Add(new ResourceViewModel { title = "leads", subtitle = "leads_description", image = "leads.png" });

		}
	}
}

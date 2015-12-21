using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace LOSS
{
	public partial class TestXAML : ContentPage
	{
        ObservableCollection<TestClass> testResource = new ObservableCollection<TestClass>();
		public TestXAML ()
		{
            TestView.ItemsSource = testResource;
            
            testResource.Add(new TestClass { DisplayName = "Rob Finnerty" });
            testResource.Add(new TestClass { DisplayName = "Bill Wrestler" });
            testResource.Add(new TestClass { DisplayName = "Dr. Geri-Beth Hooper" });
            testResource.Add(new TestClass { DisplayName = "Dr. Keith Joyce-Purdy" });
            testResource.Add(new TestClass { DisplayName = "Sheri Spruce" });
            testResource.Add(new TestClass { DisplayName = "Burt Indybrick" });
            InitializeComponent();
			
		}
	}
}


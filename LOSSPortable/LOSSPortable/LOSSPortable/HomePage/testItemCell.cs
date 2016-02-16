using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    public class testItemCell : ViewCell
    {   
        public testItemCell()
        {
            var label = new Label
            {
                YAlign = TextAlignment.Start
            };

            var  label2 = new Label
            {
                YAlign = TextAlignment.Center
            };
            
            var  label3 = new Label
            {
                YAlign = TextAlignment.Center
            };

            label.SetBinding(Label.TextProperty, "Message");
            label2.SetBinding(Label.TextProperty, "FieldNumber");
            label3.SetBinding(Label.TextProperty, "isField");

            var layout = new StackLayout
            {
                Padding = new Thickness(20, 0, 0, 0),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Children = { label, label2, label3 }
            };

            View = layout;
        }


        protected override void OnBindingContextChanged()
        {
            View.BindingContext = BindingContext;
            base.OnBindingContextChanged();
        }

    }
}

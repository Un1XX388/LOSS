using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSS
{
    public class TestListViewCode : ContentPage
    {
        public ObservableCollection<ResourceViewModel> resources { get; set; }
        public TestListViewCode()
        {
            resources = new ObservableCollection<ResourceViewModel>();
            ListView lstview = new ListView();
            this.Title = "ListView Code test";
            lstview.ItemsSource = Resources;
            lstview.ItemTemplate = new DataTemplate(typeof(CustomCell));
            Content = lstview;
            resources.Add(new ResourceViewModel { title = "accounts", subtitle = "accounts_description", image = "accounts.png" });
            resources.Add(new ResourceViewModel { title = "contacts", subtitle = "contacts_description", image = "contacts.png" });
            resources.Add(new ResourceViewModel { title = "icon", subtitle = "icon_description", image = "Icon.png" });
            resources.Add(new ResourceViewModel { title = "leads", subtitle = "leads_description", image = "leads.png" });

        }

        public class CustomCell : ViewCell
        {
            public CustomCell()
            {
                AbsoluteLayout cellView = new AbsoluteLayout() { BackgroundColor = Color.Olive };
                var nameLabel = new Label();
                nameLabel.SetBinding(Label.TextProperty, new Binding("title"));
                AbsoluteLayout.SetLayoutBounds(nameLabel,
                    new Rectangle(.25, .25, 400, 40));
                nameLabel.FontSize = 24;
                cellView.Children.Add(nameLabel);
                var typeLabel = new Label();
                typeLabel.SetBinding(Label.TextProperty, new Binding("subtitle"));
                AbsoluteLayout.SetLayoutBounds(typeLabel,
                    new Rectangle(50, 35, 200, 25));
                cellView.Children.Add(typeLabel);
                var image = new Image();
                image.SetBinding(Image.SourceProperty, new Binding("image"));
                AbsoluteLayout.SetLayoutBounds(image,
                    new Rectangle(250, .25, 200, 25));
                cellView.Children.Add(image);
                this.View = cellView;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    public class OptionsPage : ContentPage
    {
        SampleItem item;
        Label label;
        ListView listview;

        public OptionsPage()
        {
            Title = "Options";
            label = new Label();
            
            listview = new ListView();

            listview.ItemTemplate = new DataTemplate(typeof(CustomSampleCell));
            
            item = new SampleItem {Sample1 = "test string one", Sample2 = "test string two" };
            label.Text = "string before parse";
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "This is the home page!" },
                    label, listview
                }
            };
            TextOnAppearing();
        }

        protected async void TextOnAppearing()
        {
            //await App.PManager.SaveTaskAsync(item);
            listview.ItemsSource = await App.PManager.GetTaskAsync();
            label.Text = "sent to parse";
        }

        public class CustomSampleCell : ViewCell
        {
            public CustomSampleCell()
            {
                StackLayout cellview = new StackLayout() { BackgroundColor = Color.Transparent };
                cellview.Orientation = StackOrientation.Horizontal;
                var idLabel = new Label();
                idLabel.SetBinding(Label.TextProperty, new Binding("ID"));
                idLabel.TextColor = Color.White;
                cellview.Children.Add(idLabel);
                
                var sample1label = new Label();
                sample1label.SetBinding(Label.TextProperty, new Binding("Sample1"));
                idLabel.TextColor = Color.White;
                cellview.Children.Add(sample1label);

                var sample2label = new Label();
                sample2label.SetBinding(Label.TextProperty, new Binding("Sample2"));
                idLabel.TextColor = Color.White;
                cellview.Children.Add(sample2label);
                this.View = cellview;
            }
        }

        
    }
}

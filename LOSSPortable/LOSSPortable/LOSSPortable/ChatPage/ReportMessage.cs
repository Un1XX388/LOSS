using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akavache;
using System.Reactive.Linq;




//http://www.trsneed.com/just-use-akavache/

namespace LOSSPortable
{
    public class ReportMessage : ContentPage
    {
        





    public ReportMessage(Message msg)
        {
            Message ab = new Message();
            ab.setMessage("POSEIDON QUIVERS BEFORE ME");
            ab.setId("12345");

            Button store = new Button { Text = " Store", WidthRequest = 100, HeightRequest = 50, TextColor = Color.Black, BackgroundColor = Color.FromHex("B3B3B3"), BorderColor = Color.Black, FontAttributes = FontAttributes.Bold, Font = Font.OfSize("Arial", 22) };
            Button load = new Button { Text = " Load", WidthRequest = 100, HeightRequest = 50, TextColor = Color.Black, BackgroundColor = Color.FromHex("B3B3B3"), BorderColor = Color.Black, FontAttributes = FontAttributes.Bold, Font = Font.OfSize("Arial", 22) };
            store.Clicked += async delegate { await Store("12345", ab); }; 
            load.Clicked += async delegate { Message ab2= await Get("12345"); DisplayAlert(ab2.getId(), ab2.getMessage(), "ok"); };

            Title = "Report Message from: " + msg.getSender();
            var result = "";


            Button picker = new Button { Text = " Select Reason ", WidthRequest = 100, HeightRequest = 50, TextColor = Color.Black, BackgroundColor = Color.FromHex("B3B3B3"), BorderColor = Color.Black, FontAttributes = FontAttributes.Bold, Font = Font.OfSize("Arial", 22) };
            picker.Clicked += async delegate
            {
                result = await DisplayActionSheet(null, "Cancel", null, "Offensive Language", "Spam", "Threat", "Solicitation");
                picker.Text = result;
            };

            var editor = new Entry {  Placeholder = "Enter explanation: " };
            
            Button report = new Button { Text = " Report ", WidthRequest = 100, HeightRequest = 50, TextColor = Color.Black, BackgroundColor = Color.FromHex("B3B3B3"), BorderColor = Color.Black, FontAttributes = FontAttributes.Bold, Font = Font.OfSize("Arial", 22) };
            report.Clicked += delegate
            {
                DisplayAlert("Review Report", "" + msg.getMessage()+" "+result+" " +editor.Text, "Report");
            };
                
            StackLayout layout = new StackLayout
            {
                Children =
                        {   picker,
                            editor,
                            report,
                            store,
                            load
                        }
            };
            this.Content = layout;            


        }

        public async Task<Message> Get(string key)
        {
            try
            {
                return await BlobCache.LocalMachine.GetObject<Message>(key);
            }
            catch (KeyNotFoundException)
            {
                return default(Message);

            }
        }

        public async Task Store<Message>(string key, Message value)
        {
            await BlobCache.LocalMachine.InsertObject(key, value);
            
        }

    }
}


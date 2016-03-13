using Acr.UserDialogs;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;




//http://www.trsneed.com/just-use-akavache/

namespace LOSSPortable
{
    public class ReportMessage : ContentPage
    {

        CancellationTokenSource ct = new CancellationTokenSource();
        ReportM temp;
        Message msg;
        Editor editor;
        String result = "";



        public ReportMessage(Message msg)
        {
            temp = new ReportM();
            this.msg = msg;

            Title = "Report Message from: " + msg.getSender();
            //var result = "";


            Button picker = new Button { Text = " Select Reason ", WidthRequest = 100, HeightRequest = 50, TextColor = Color.Black, BackgroundColor = Color.FromHex("B3B3B3"), BorderColor = Color.Black, FontAttributes = FontAttributes.Bold, Font = Font.OfSize("Arial", 22) };
            picker.Clicked += async delegate
            {
                result = await DisplayActionSheet(null, "Cancel", null, "Offensive Language", "Spam", "Threat", "Solicitation");
                if (result != "" && result !="Cancel")
                    picker.Text = result;
            };
            Label exp = new Label
            {
                Text = "Please select a reason and enter description.",
                TextColor = Color.Red,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
            };

            Label exp2 = new Label
            {
                Text = "Enter description.",
                TextColor = Color.Red,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
            };
            editor = new Editor { Text = "" };

            Button report = new Button { Text = " Report ", WidthRequest = 100, HeightRequest = 50, TextColor = Color.Black, BackgroundColor = Color.FromHex("B3B3B3"), BorderColor = Color.Black, FontAttributes = FontAttributes.Bold, Font = Font.OfSize("Arial", 22) };
            report.Clicked += onSubmitPressed; 
            
                
            StackLayout layout = new StackLayout
            {
                Children =
                        {   exp,
                            picker,
                            exp2,
                            editor,
                            report
                        }
            };
            this.Content = layout;


        }

        async void onSubmitPressed(Object sender, EventArgs e)
        {
            temp.Comment = editor.Text ;
            temp.Message = msg.getMessage();
            temp.From = msg.getSender();
            temp.To = msg.getReciever();
            temp.id = AmazonUtils.Credentials.GetIdentityId();
            temp.Type = result;
            temp.Date = msg.date; //change to message's time/date

            System.Diagnostics.Debug.WriteLine("Review Report" + temp.Comment + temp.Message + temp.From + temp.To + temp.id + temp.Type  + temp.Date);
            //send to server: 

            await SaveAsync<ReportM>(temp, ct.Token);

            UserDialogs.Instance.ShowSuccess("Thank you! Message report has been submitted.");
            await Navigation.PopAsync();

        }

        public async Task SaveAsync<ReportM>(ReportM entity, CancellationToken ct)
        {
            var context = AmazonUtils.DDBContext;
            await context.SaveAsync<ReportM>(entity, ct);
            System.Diagnostics.Debug.WriteLine("entity saved");
        }
    }
}

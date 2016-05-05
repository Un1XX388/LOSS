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
        ChatMessage msg;
        Editor editor;
        //String result = "";
        String reportType;



        public ReportMessage(ChatMessage msg) //general constructor for report message 
        {
            temp = new ReportM();
            this.msg = msg;

            Title = "Report Message from: " + Constants.conv.name;
            //var result = "";


            /*Button picker = new Button { Text = " Select Reason ", WidthRequest = 100, HeightRequest = 50, TextColor = Color.Black, BackgroundColor = Color.FromHex("B3B3B3"), BorderColor = Color.Black, FontAttributes = FontAttributes.Bold, Font = Font.OfSize("Arial", 22) };
            picker.Clicked += async delegate
            {
                result = await DisplayActionSheet(null, "Cancel", null, "Offensive Language", "Spam", "Threat", "Solicitation");
                if (result != "" && result !="Cancel")
                    picker.Text = result;
            };
            */
            Picker picker = new Picker
            {
                Title = "Select a Reason:",
                BackgroundColor = Color.Default

            };

            picker.Items.Add("Offensive Language");
            picker.Items.Add("Spam");
            picker.Items.Add("Threat");
            picker.Items.Add("Solicitation");

            picker.SelectedIndexChanged += (sender, args) =>
            {
                reportType = picker.Items[picker.SelectedIndex];
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

        async void onSubmitPressed(Object sender, EventArgs e) //sending a report of a message. creates a report object and sends it to the server 
        {
            temp.Comment = editor.Text;
            temp.Message = msg.Text;
            temp.From = msg.Sender;
            temp.To = msg.Reciever;
            temp.id = AmazonUtils.Credentials.GetIdentityId();
            temp.Type = reportType;
            temp.Date = msg.Date; //change to message's time/date

            System.Diagnostics.Debug.WriteLine("Review Report" + temp.Comment + temp.Message + temp.From + temp.To + temp.id + temp.Type  + temp.Date);
            //send to server: 

            await SaveAsync<ReportM>(temp, ct.Token);

            UserDialogs.Instance.ShowSuccess("Thank you! Message report has been submitted.");
            
            await Navigation.PopAsync();
        }

        public async Task SaveAsync<ReportM>(ReportM entity, CancellationToken ct)
        {
            try {
                var context = AmazonUtils.DDBContext;
                await context.SaveAsync<ReportM>(entity, ct);
                System.Diagnostics.Debug.WriteLine("entity saved");

            }
            catch (Exception E)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + E);
            }
            

        }
    }
}

using Acr.UserDialogs;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    public class ReportPage: ContentPage
    {
        String reportType;
        Editor reason;
        StackLayout mainContent;
        Label errorMessage;
        CancellationTokenSource ct = new CancellationTokenSource();
        Label label = new Label();
        Label label2 = new Label();

        public ReportPage()
        {

            //sets the background color based on settings
            if (Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;
            }

            Title = "Report A Problem";
            this.Padding = new Thickness(5, Device.OnPlatform(20, 5, 5), 5, 5);


            //===================Equivalent of drop down list to choose report options from ================
            Picker picker = new Picker
            {
                Title = "Select a Reason:",
                BackgroundColor = Color.Default
                
            };

            picker.Items.Add("Report Bugs and Other Issues");
            picker.Items.Add("Report Content");
            picker.Items.Add("Send Feedback");

            picker.SelectedIndexChanged += (sender, args) =>
            {
                reportType = picker.Items[picker.SelectedIndex];                
            };

            //==============================================================================================
            label.Text = "Select A Report Type: ";
            label.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));

            label2.Text = "\nBriefly explain your reason: \n";
            label2.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            //========================== Editor for Multiple Lines of Text ==============================

            reason = new Editor {
              //  Placeholder = "Briefly explain your reason: ",
                BackgroundColor = Color.White,
                
                TextColor = Color.Black,
                MinimumHeightRequest = 50
                
            };

            //========================== Submit Button =========================================================

            Button submitButton = new Button
            {
                Text = "Submit",
                FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Button)),
                TextColor = Color.White,
                WidthRequest = 150,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            submitButton.Clicked += onSubmitPressed;

            //=========================== Error Message if needed ==============================================
            errorMessage = new Label
            {
                Text = "Please select a reason and enter description.",
                TextColor = Color.Red,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
            };

            mainContent = new StackLayout
            {
                Children = { label, picker, label2, reason, submitButton }

            };
            this.Content = mainContent;
        }

        //============================================ FUNCTIONS ===============================================

        public async Task SaveAsync<ReportProblem> (ReportProblem entity, CancellationToken ct)
        {
            var context = AmazonUtils.DDBContext;
            await context.SaveAsync<ReportProblem> (entity, ct);
            System.Diagnostics.Debug.WriteLine("entity saved");
        }

        //submit function - checks if the form is filled, otherwise prompts user to complete it

     
        async void onSubmitPressed(object sender, EventArgs e)
        {

            if((reason.Text != null) && (reportType != null))
            {
                ReportProblem temp = new ReportProblem();

                temp.id = AmazonUtils.Credentials.GetIdentityId(); ;
                temp.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
                temp.Message = reason.Text;
                temp.Type = reportType;

                // save report on server
                await SaveAsync<ReportProblem>(temp, ct.Token);
        
                UserDialogs.Instance.ShowSuccess("Thank you! Your feedback has been submitted.");
                await Navigation.PopAsync();
            }
            else
            {
                mainContent.Children.Add(errorMessage);
                this.Content = mainContent;
            }
            
        }
    }
}

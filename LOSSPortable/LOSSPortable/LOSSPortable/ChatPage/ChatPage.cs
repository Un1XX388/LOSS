using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LOSSPortable
{
	public class ChatPage : ContentPage
	{   private Label Output;
        private Editor editor;
        private Button send;
        List<string> Messages = new List<string>();
        private StackLayout stackLayout;
        private StackLayout outerStack;
        private ScrollView scroll;
        private ScrollView innerScroll;

        public ChatPage ()
		{
            
            

            Title = "Chat";
			Icon = "Accounts.png";

            //entry:
            var editor = new Entry();
            editor.Placeholder = "Enter Message: ";

            var label = new Label { Text = "Message a Volunteer",FontSize=30 , BackgroundColor = Color.Aqua, TextColor = Color.White, XAlign = TextAlignment.Center };
            //editor = new Editor { Text = "Enter Message: ", BackgroundColor= Colors.lightblue, FontSize = 20, HeightRequest = 50 };
            send = new Button { Text = "Send" };
            

            send.Clicked += delegate {

                String mes = editor.Text;
                // Output.Text = "Output: " + mes;
                try
                {
                    Messages.Add(mes);
                }
                catch (NullReferenceException ex)
                {
                    DisplayAlert("Alert", "Processor Usage" + ex.Message + mes, "OK");

                }

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (s, e) => {
                    // handle the tap
                };

                //on click event:
                var tgr = new TapGestureRecognizer();
                tgr.Tapped += (s, e) => OnLabelClicked();

                Label sender = new Label { Text = "Self"+"\n"+ mes + "\t", BackgroundColor = Color.White, TextColor = Color.Purple, FontAttributes = FontAttributes.Bold, XAlign = TextAlignment.End };
                Label response = new Label { Text = "Response" + "\n\t" + mes, BackgroundColor = Color.White, TextColor = Color.Purple, FontAttributes = FontAttributes.Bold, XAlign = TextAlignment.Start };

                sender.GestureRecognizers.Add(tgr);
                response.GestureRecognizers.Add(tgr);

                stackLayout.Children.Add(sender);
                stackLayout.Children.Add(response);
                

                this.Content = outerStack;
            };

                Device.BeginInvokeOnMainThread(() =>   //automatically updates
            {
                stackLayout = new StackLayout
                {  
                    Children =
                    {
                        label

                    }
                };

                innerScroll = new ScrollView
                {
                    Padding = new Thickness(10, 10, 10, 20),
                    BackgroundColor = Color.White,
                    Content = stackLayout,
                    


                };

                stackLayout.BackgroundColor = Color.White;

                outerStack = new StackLayout
                {
                    Spacing = 0,
                    Children =
                            {
                                //label,
                                innerScroll,
                                editor,
                                send
                            }
                    
                };

                innerScroll.HeightRequest = 440;
                
             
                Content = outerStack;
                Title = "Message a Volunteer";
            });
           
        }

        //individual message tapped in chat:
        private void OnLabelClicked()
        {   //https://developer.xamarin.com/guides/xamarin-forms/user-interface/listview/interactivity/
            DisplayAlert("Alert", "Clicked item!" , "OK");

        }




     

    }
	
}
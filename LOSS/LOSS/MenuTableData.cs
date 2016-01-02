using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace LOSS{

	public class MenuTableData : List<MenuItem>{
		public MenuTableData(){
			this.Add(new MenuItem(){
				Title = "Account Preferences",
				IconSource = "contacts.png",
				TargetType = typeof(ContractsPage)
			});

			this.Add(new MenuItem(){
				Title = "Resources",
				IconSource = "leads.png",
				TargetType = typeof(ResourcesPage)
			});

			this.Add(new MenuItem(){
				Title = "Chat",
				IconSource = "accounts.png",
				TargetType = typeof(ChatPage)
			});

			this.Add(new MenuItem(){
				Title = "Feedback",
				IconSource = "opportunities.png",
				TargetType = typeof(FeedbackPage)
			});

			this.Add(new MenuItem(){
				Title = "Settings",
				IconSource = "opportunities.png",
				TargetType = typeof(SettingsPage)
			});

			this.Add(new MenuItem(){
				Title = "Hotline",
				IconSource = "opportunities.png",
				TargetType = typeof(SettingsPage)
			});
		}// End of constructor MenuTableData().
	}// End of class MenuTableData.
}// End of namespace LOSS.
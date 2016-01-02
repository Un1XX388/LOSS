using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace LOSS{
	public class MenuListData : List<MenuItem>{
		public MenuListData(){
			this.Add(new MenuItem(){
				Title			= "Account Preferences",
				IconSource		= "contacts.png",
				TargetType		= typeof(AccountPreferencesPage)
			});

			this.Add(new MenuItem(){
				Title			= "Resources",
				IconSource		= "leads.png",
				TargetType		= typeof(ResourcesPage)
			});

			this.Add(new MenuItem(){
				Title			= "Chat",
				IconSource		= "accounts.png",
				TargetType		= typeof(ChatPage)
			});

			this.Add(new MenuItem(){
				Title			= "Feedback",
				IconSource		= "opportunities.png",
				TargetType		= typeof(FeedbackPage)
			});

			this.Add(new MenuItem(){
				Title			= "Settings",
				IconSource		= "opportunities.png",
				TargetType		= typeof(SettingsPage)
			});
		}// End of MenuListData() method.
	}// End of MenuListData class.
}// End of namespace LOSS.
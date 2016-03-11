using System;
using System.ComponentModel;


namespace LOSSPortable{
	public class OnlineResourceViewModel : INotifyPropertyChanged{
		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
		
		public string	Image		{ get; set; }
		public string	Title		{ get; set; }
		public string	Description	{ get; set; }
		public string	Link		{ get; set; }
		public string	Fav			{ get; set; }

		public OnlineResourceViewModel(){
		}// End of OnlineResourceViewModel() method.

		public override string ToString(){
			return Image + ',' + Title + ',' + Description + ',' + Link + ',' + Fav;
		}// End of ToString() method.

	}// End of OnlineResourceViewModel class.
}// End of LOSSPortable namespace.
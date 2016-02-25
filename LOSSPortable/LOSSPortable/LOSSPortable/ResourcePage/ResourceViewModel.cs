using System;
using System.ComponentModel;

namespace LOSSPortable{
	public class ResourceViewModel : INotifyPropertyChanged{
		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
		
		public string	Image		{ get; set; }
		public string	Title		{ get; set; }
		public string	Description	{ get; set; }
		public string	Link		{ get; set; }

		public ResourceViewModel(){
		}// End of ResourceViewModel() method.

		public override string ToString(){
			return Image + ',' + Title + ',' + Description + ',' + Link;
		}// End of ToString() method.
	}// End of ResourceViewModel class.
}// End of LOSS namespace.

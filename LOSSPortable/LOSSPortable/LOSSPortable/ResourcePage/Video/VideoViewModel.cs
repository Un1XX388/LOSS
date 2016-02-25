using System;
using System.ComponentModel;

namespace LOSSPortable{
	public class VideoViewModel : ResourceViewModel, INotifyPropertyChanged{
		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		public string	Image		{ get; set; }

		public VideoViewModel(){
		}// End of VideoViewModel() method.

		public override string ToString(){
			return Image + ',' + Title + ',' + Description + ',' + Link;
		}// End of ToString() method.
	}// End of VideoViewModel class.
}// End of LOSS namespace.

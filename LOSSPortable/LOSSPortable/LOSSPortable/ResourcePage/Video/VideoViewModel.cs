using System;
using System.ComponentModel;

namespace LOSSPortable
{
	public class VideoViewModel : INotifyPropertyChanged{
		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		public string	Image		{ get; set; }
		public string	Title		{ get; set; }
		public string	Description	{ get; set; }
		public string	Link		{ get; set; }

		public VideoViewModel(){
		}// End of VidowViewModel() method.
	}// End of VideoViewModel class.
}// End of LOSS namespace.

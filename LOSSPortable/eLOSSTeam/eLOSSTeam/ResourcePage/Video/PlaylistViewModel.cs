using System;
using System.ComponentModel;

namespace LOSSPortable
{
	public class PlaylistViewModel : INotifyPropertyChanged{
		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		public string	Image		{ get; set; }
		public string	Name		{ get; set; }
		public string	Description	{ get; set; }
		public Type		TargetType	{ get; set; }
		public PlaylistViewModel(){
		}
	}
}
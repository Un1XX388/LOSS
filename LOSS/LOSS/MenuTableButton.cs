using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace LOSS{
	public class MenuTableButton{
		// Title of item.
		public string	Title{ get; set; }
		// Data source of item.
		public string	IconSource{ get; set; }
		// Type of item.
		public Type		TargetType{ get; set; }
	}// End of MenuTableButton class.
}// End of namespace LOSS.
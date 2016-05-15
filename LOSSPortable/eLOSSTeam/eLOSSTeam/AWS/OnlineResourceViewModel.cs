using Amazon.DynamoDBv2.DataModel;
using System;
using System.ComponentModel;


namespace LOSSPortable{

    [DynamoDBTable("Resource")]
    public class OnlineResourceViewModel : INotifyPropertyChanged{
		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
		
		public string	Image		{ get; set; }

        [DynamoDBProperty]
        public string	Title		{ get; set; }

        [DynamoDBProperty]
        public string	Description	{ get; set; }

        [DynamoDBProperty]
        public string Type { get; set; }

        [DynamoDBHashKey]
        public string	Link		{ get; set; }

        public string	Fav			{ get; set; }

		public OnlineResourceViewModel(){
		}// End of OnlineResourceViewModel() method.

		public override string ToString(){
			return Image + ',' + Title + ',' + Description + ',' + Link + ',' + Fav;
		}// End of ToString() method.

	}// End of OnlineResourceViewModel class.
}// End of LOSSPortable namespace.
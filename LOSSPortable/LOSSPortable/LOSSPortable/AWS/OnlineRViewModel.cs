using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOSSPortable
{
    [DynamoDBTable("Resource")]
    public class OnlineRViewModel: INotifyPropertyChanged 
    {
        [DynamoDBHashKey]
        public string URL { get; set; }

        [DynamoDBProperty]
        public string Title { get; set; }

        [DynamoDBProperty]
        public string Type { get; set; }

        [DynamoDBProperty]
        public string Description { get; set; }

        public string Image { get; set; }
        public int count { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public OnlineRViewModel()
        {
        }// End of OnlineResourceViewModel() method.

        public override string ToString()
        {
            return Image + ',' + Title + ',' + Description + ',' + URL + ',' + Type + ',' + count;
        }// End of ToString() method.
    }
}

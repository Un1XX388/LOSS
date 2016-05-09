using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOSSPortable
{
    //This class is used by OnlineResources Page

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

        public event PropertyChangedEventHandler PropertyChanged;

        public OnlineRViewModel()
        {
        }// End of OnlineResourceViewModel() method.
    }
}

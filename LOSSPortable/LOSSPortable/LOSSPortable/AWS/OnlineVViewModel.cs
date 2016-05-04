using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    [DynamoDBTable("VideoResources")]
    public class OnlineVViewModel : INotifyPropertyChanged
    {
        [DynamoDBHashKey]
        public string URL { get; set; }

        [DynamoDBProperty]
        public string Title { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey]
        public string Playlist { get; set; }

        [DynamoDBProperty]
        public string Description { get; set; }

        [DynamoDBProperty]
        public string Image { get; set; }

        private Image image;

        public Image imageLoad { 
            get{
                if (image == null)
                {
                    image = new Image { Aspect = Aspect.AspectFit };
                    image.Source = new UriImageSource
                    {
                        Uri = new Uri(Image),
                        CachingEnabled = true,
                        CacheValidity = new TimeSpan(5, 0, 0, 0)
                    };
                }
                return image;
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        }
}

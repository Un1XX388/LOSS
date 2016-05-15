using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eLOSSTeam
{
    //This class is used by VideoPage 

    [DynamoDBTable("Playlist")]
    public class OnlinePlaylistModel : INotifyPropertyChanged
    {
        [DynamoDBHashKey]
        public string ID { get; set; }

        [DynamoDBProperty]
        public string Title { get; set; }

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

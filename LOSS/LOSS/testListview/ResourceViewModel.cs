using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace LOSS
{
    public class ResourceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string title { get; set; }
        public string subtitle { get; set; }
        public string image {get; set;}
        
        public ResourceViewModel()
        {
        }
    }
}

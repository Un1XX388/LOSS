using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    public interface iSNS
    {
        event EventHandler newMessage;
        event EventHandler onChatPage;
    }
}

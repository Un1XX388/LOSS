using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace LOSS
{
    public class Fragment1 : Android.Support.V4.App.Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.NavDrawerFrag, null);

            view.FindViewById<Button>(Resource.Id.button).Click += (sender, args) =>
            {
                var popUp = new Android.Support.V7.Widget.PopupMenu(Activity, (Button)sender);
                popUp.Inflate(Resource.Menu.action_menu);
                popUp.Show();
            };

            view.FindViewById<TextView>(Resource.Id.main_text).Text = "This is BROWSE FRAGMENT";
            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.action_menu, menu);
        }
    }

  

 
}
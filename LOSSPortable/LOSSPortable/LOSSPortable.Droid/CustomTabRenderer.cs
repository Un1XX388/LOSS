using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(LOSSPortable.Droid.CustomTabRenderer))]

namespace LOSSPortable.Droid
{
    public class CustomTabRenderer : TabbedRenderer, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private Activity _activity;
        Boolean _isFirstDesign = true;

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);
            ViewTreeObserver.RemoveOnGlobalLayoutListener(this);

            _activity = this.Context as Activity;
        }

        public void OnGlobalLayout()
        {
            if (_isFirstDesign)
            {
                ActionBar actionBar = _activity.ActionBar;
                ActionBarTabsSetup(actionBar);
               // Log.Info("Tab count: ", "Total:" + _activity.ActionBar.TabCount);
            }
        }

        private void ActionBarTabsSetup(ActionBar actionBar)
        {
            if (actionBar.TabCount == 0) return;

            if (this.Element.GetType() == typeof(ResourcesTabbedSwipePage))
            {
                if (actionBar.TabCount < 3) return;

                ActionBar.Tab webTab = actionBar.GetTabAt(0);
                webTab.SetIcon(Resource.Drawable.web);

                ActionBar.Tab playlistTab = actionBar.GetTabAt(1);
                playlistTab.SetIcon(Resource.Drawable.playlist);

                ActionBar.Tab watchedTab = actionBar.GetTabAt(2);
                watchedTab.SetIcon(Resource.Drawable.watched);


                // To ensure selected tab's icon is high-lighted
                //actionBar.SelectTab(myAddress);
                //actionBar.SelectTab(myProfile);

                _isFirstDesign = false;
            }
        }
        // May put this code in a different method - was just for testing
        public override void OnWindowFocusChanged(bool hasWindowFocus)
        {
            // Here the magic happens:  get your ActionBar and select the tab you want to add an image
            ActionBar actionBar = _activity.ActionBar;

            if (actionBar.TabCount > 0)
            {
                Android.App.ActionBar.Tab tabOne = actionBar.GetTabAt(0);

                tabOne.SetIcon(Resource.Drawable.web);
            }
            base.OnWindowFocusChanged(hasWindowFocus);
        }
    }
}
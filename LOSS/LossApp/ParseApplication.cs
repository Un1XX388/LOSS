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

using Parse;

namespace LOSS {
  [Application]
  class ParseApplication : Application {
    public ParseApplication(IntPtr handle, JniHandleOwnership ownerShip)
      : base(handle, ownerShip) {
    }

    public override void OnCreate() {
      base.OnCreate();

      ParseClient.Initialize("4WZ6EjSYvyJNPCAjC63mbRyjhw5N1yhJyuYOHdjB",
                           "iQRI72u6OgEJxUmFcX0pH4xj5cS90P6cz2ugmyYk");
    }
  }
}
using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;


namespace LOSS
{
    public static class Colors
    {
        /*
        * to reference a color in xaml, Color="{x:Static local:Colors.purple}"
        * make sure to include ' xmlns:local="clr-namespace:LOSS;assembly=LOSS" ' in the xaml just below
        * xmlns="http://xamarin.com/schemas/2014/forms" and xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
        * 
        * to reference a color in C# class, just type '= Colors.purple'; for example, assuming namespace is LOSS.
        */

        public static Color purple = Color.FromHex("8C489F");
        public static Color white = Color.FromHex("F1F0FF");
        public static Color lightblue = Color.FromHex("C3C3E5");
        public static Color darkblue = Color.FromHex("443266");
    }
}

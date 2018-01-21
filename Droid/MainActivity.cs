using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace MuscleFellow.Droid
{
    [Activity(Label = "MuscleFellow.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            // https://stackoverflow.com/questions/5528850/how-to-connect-localhost-in-android-emulator
            App.Host = "http://10.0.2.2:5001"; //安卓模拟器需要使用10.0.2.2访问localhost
            LoadApplication(new App());
        }
    }
}

using System;
using System.IO;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Environment = System.Environment;

namespace TestFormsApp.Droid
{
    [Activity(Label = "TestFormsApp", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            string content;
            using (StreamReader sr = new StreamReader(Assets.Open("Languages.csv")))
            {
                content = sr.ReadToEnd();
            }

            Plugin.Localization.CrossLocalization.Current.LoadLanguagesFromString(content);


            LoadApplication(new TestFormsApp.App());
        }
    }
}


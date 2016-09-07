using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Plugin.Localization;
using Xamarin.Forms;


namespace TestFormsApp
{
    public class App : Application
    {
        public App()
        {


            B_Clicked(null, null);
            var button = new Button();
            button.Text = Plugin.Localizing.Localization.Current["message2"];
            button.Clicked += B_Clicked;

            

            // The root page of your application
            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Label
                        {
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = Plugin.Localizing.Localization.Current["message1"]
                        },
                        button,
                    }
                }
            };
        }

        private void B_Clicked(object sender, EventArgs e)
        {
            if(Plugin.Localizing.Localization.Current.CurrentCulture == "en-US")
            {
                Plugin.Localizing.Localization.Current.CurrentCulture = "ru-ru";
            }
            else
            {
                Plugin.Localizing.Localization.Current.CurrentCulture = "en-US";
            }





        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
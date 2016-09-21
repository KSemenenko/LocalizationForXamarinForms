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
            button.Text = CrossLocalization.Current["message2"];
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
                            Text = CrossLocalization.Current["message1"]
                        },
                        button,
                    }
                }
            };
        }

        private void B_Clicked(object sender, EventArgs e)
        {
            if(CrossLocalization.Current.CurrentCulture == "en-US")
            {
                CrossLocalization.Current.CurrentCulture = "ru-ru";
            }
            else
            {
                CrossLocalization.Current.CurrentCulture = "en-US";
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
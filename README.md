# Localization Plugin for Xamarin and Windows
This project is a cross platform library for Xamarin Forms, which enables a handy use of localization in your applications.  


Should you have any comments or suggestions, please let me know. Let's make it an easy-to-use tool for our projects.


## Available at NuGet. 
https://www.nuget.org/packages/ksemenenko.Localization/


|Platform|Supported|Version|
| ------------------- | :-----------: | :------------------: |
|Xamarin.iOS|Partial|iOS 6+|
|Xamarin.iOS Unified|Yes|iOS 6+|
|Xamarin.Android|Yes|API 10+|
|Windows Phone 8|Partial|8.0+|
|Windows Phone 8.1|Yes|8.1+|
|Windows Store|Yes|8.1+|
|Windows 10 UWP|Yes|10+|
|Xamarin.Mac|Partial||



## Example csv structure:
|default:en-us|en-us|ru-ru|
| ------------------- | :-----------: | :------------------: |
|MainMenu_News|News|Новости|
|MainMenu_Home|Home|Домой|
|STRING_NAME|ENG_VALUE|RU_VALUE|


## Example use:

#### Init:

```cs
//load file from resources
Stream stream = assembly.GetManifestResourceStream(resourcePrefix + "Languages.csv");
string text = string.Empty;
using(var reader = new StreamReader(stream))
{
    text = reader.ReadToEnd();
}
CrossLocalization.Current.LoadLanguagesFromString(text);

//you can set the culture at hand
CrossLocalization.Current.CurrentCulture = "ru-ru";

//To remove unused languages from runtime
CrossLocalization.Current.LeaveUnusedLanguages = false;

//get localize value
var localizeValue = CrossLocalization.Current["MainMenu_News"];

```

## For Xamarin Forms 
You can create IMarkupExtension like this:
```cs
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plugin.Localization
{
    [ContentProperty("Source")]
    public class LangugeExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if(Source == null)
            {
                return null;
            }

            return CrossLocalization.Current[Source];
        }
    }
}
```

and them use in xaml:
```xml
<?xml version="1.0" encoding="utf-8"?>

<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MobileApp.View.TestPage"
             xmlns:localization="using:Plugin.Localization"
             BackgroundColor="White">
  <Grid>
    <Button Text="{localization:Languge MainMenu_News}"/>
  </Grid>
</ContentPage>
```



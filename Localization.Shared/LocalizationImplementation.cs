using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using Plugin.Localization.Abstractions;

namespace Plugin.Localization
{
    public partial class LocalizationImplementation : ILocalization
    {
        private Dictionary<string, string> languageDictionary = new Dictionary<string, string>();

        public LocalizationImplementation()
        {
            
        }

        public LocalizationImplementation(CultureInfo cultureInfo)
        {
            CurrentCultureInfo = cultureInfo;
        }

        public LocalizationImplementation(string cultureInfo)
        {
            CurrentCulture = cultureInfo;
        }

        public CultureInfo CurrentCultureInfo { get; set; } = CultureInfo.InvariantCulture;

        public void LoadLocalFile(string path)
        {
            var content = FileLoad(path);
        }

        public string this[string key]
        {
            get
            {
                return ";";
                
            }
            set
            {
                
            }
        }

        public string CurrentCulture
        {
            get { return CurrentCultureInfo.Name; }
            set
            {
                CurrentCultureInfo = new CultureInfo(value);
            }
        }

        

    }
}
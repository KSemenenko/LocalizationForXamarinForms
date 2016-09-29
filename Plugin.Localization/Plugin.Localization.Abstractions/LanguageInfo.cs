using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.Localization.Abstractions
{
    public class LanguageInfo
    {
        public string DisplayName { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
        public string NativeName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string TwoLetterName { get; set; } = string.Empty;
    }
}

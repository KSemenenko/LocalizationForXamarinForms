using System;
using System.Globalization;

namespace Plugin.Localization.Abstractions
{
    /// <summary>
    ///     Interface for Plugin.Localization
    /// </summary>
    public interface ILocalization
    {
        void LoadLanguagesFromFile(string path);

        void LoadLanguagesFromString(string content);

        string this[string key] { get; set; }

        string CurrentCulture { get; set; }

        CultureInfo CurrentCultureInfo{ get; set; }
    }
}
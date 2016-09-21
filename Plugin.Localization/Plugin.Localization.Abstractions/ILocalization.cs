using System;
using System.Globalization;

namespace Plugin.Localization.Abstractions
{
    /// <summary>
    ///     Interface for Plugin.Localization
    /// </summary>
    public interface ILocalization
    {
        /// <summary>
        /// Load CSV file from file system.
        /// </summary>
        /// <param name="path"></param>
        void LoadLanguagesFromFile(string path);

        void LoadLanguagesFromString(string content);

        string this[string key] { get; }

        string CurrentCulture { get; set; }

        CultureInfo CurrentCultureInfo { get; set; }

        /// <summary>
        /// Leave unused languages.
        /// </summary>
        bool LeaveUnusedLanguages { get; set; }
    }
}
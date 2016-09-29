using System;
using System.Collections;
using System.Collections.Generic;
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

        CultureInfo CurrentCulture { get; set; }

        /// <summary>
        /// Leave unused languages.
        /// </summary>
        bool LeaveUnusedLanguages { get; set; }

        dynamic Dynamic { get; }

        IList<LanguageInfo> Languages { get; }
    }
}
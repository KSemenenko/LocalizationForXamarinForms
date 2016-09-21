﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using Localization.Shared.Parsers;
using Plugin.Localization.Abstractions;

namespace Plugin.Localization
{
    public partial class LocalizationImplementation : ILocalization
    {
        private Dictionary<string, Dictionary<string, string>> languageDictionary = new Dictionary<string, Dictionary<string, string>>();
        private object sync = new object();
        private string defaultCultureCode = string.Empty;
        private const string DefaultDelimeter = "default:";

        public LocalizationImplementation()
        {
            CurrentCultureInfo = CultureInfo.CurrentCulture;
            Delimiter = string.Empty;
        }

        public LocalizationImplementation(CultureInfo cultureInfo)
        {
            CurrentCultureInfo = cultureInfo;
        }

        public LocalizationImplementation(string cultureInfo)
        {
            CurrentCulture = cultureInfo;
        }

        public CultureInfo CurrentCultureInfo { get; set; }

        public string Delimiter { get; set; }

        public void LoadLanguagesFromFile(string path)
        {
            var content = FileLoad(path);
            LoadLanguages(content);
        }

        public void LoadLanguagesFromString(string content)
        {
            LoadLanguages(content);
        }

        private void LoadLanguages(string content)
        {
            CsvFileReader reader = string.IsNullOrEmpty(Delimiter) ? new CsvFileReader(content) : new CsvFileReader(content, Delimiter[0]);

            MakeDictionary(reader);
            FillDictionary(reader);
            ClearDictionary();
        }

        private void MakeDictionary(CsvFileReader reader)
        {
            languageDictionary = new Dictionary<string, Dictionary<string, string>>();
            foreach(var header in reader.ReadHeader())
            {
                if(!string.IsNullOrEmpty(header))
                {
                    if(header.ToLowerInvariant().StartsWith(DefaultDelimeter))
                    {
                        defaultCultureCode = header.Split(new string[] {DefaultDelimeter}, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? string.Empty;
                    }
                    else
                    {
                        languageDictionary.Add(header.ToLowerInvariant(), new Dictionary<string, string>());
                    }
                }
            }
        }

        private void ClearDictionary()
        {
            if(!LeaveUnusedLanguages)
            {
                var items = languageDictionary.Where(w => w.Key != CurrentCulture).ToList();
                foreach(var item in items)
                {
                    languageDictionary.Remove(item.Key);
                }
            }
        }

        private void FillDictionary(CsvFileReader reader)
        {
            foreach(var row in reader.ReadRows())
            {
                int count = 1;
                foreach(var item in languageDictionary)
                {
                    item.Value.Add(row[0], row[count]);
                    count++;
                }
            }
        }

        private Dictionary<string, string> GetCurrentCultureDictionary(string culture, bool firstRequest = true)
        {
            Dictionary<string, string> langDictionary;
            if(!languageDictionary.TryGetValue(CurrentCulture, out langDictionary))
            {
                langDictionary = languageDictionary.FirstOrDefault().Value;

                if(langDictionary == null)
                {
                    if(!string.IsNullOrEmpty(defaultCultureCode) && firstRequest)
                    {
                        return GetCurrentCultureDictionary(defaultCultureCode, false);
                    }
                    else
                    {
                        return new Dictionary<string, string>();
                    }
                }
            }

            return langDictionary;
        }

        public string this[string key]
        {
            get
            {
                Dictionary<string, string> langDictionary = GetCurrentCultureDictionary(CurrentCulture);
                string message;
                if(langDictionary.TryGetValue(key, out message))
                {
                    return message;
                }

                return string.Empty;
            }
        }

        public string CurrentCulture
        {
            get { return CurrentCultureInfo?.Name.ToLowerInvariant(); }
            set { CurrentCultureInfo = new CultureInfo(value); }
        }

        public bool LeaveUnusedLanguages { get; set; } = true;
    }
}
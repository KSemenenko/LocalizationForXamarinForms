using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using Localization.Shared.Parsers;
using Plugin.Localization.Abstractions;

namespace Plugin.Localization
{
    public partial class LocalizationImplementation : ILocalization
    {
        private const string DefaultDelimeter = "default:";
        private CultureInfo defaultCulture;
        private Dictionary<CultureInfo, Dictionary<string, string>> languageDictionary = new Dictionary<CultureInfo, Dictionary<string, string>>();

        public LocalizationImplementation()
        {
            CurrentCulture = CultureInfo.CurrentCulture;
        }

        public LocalizationImplementation(CultureInfo cultureInfo)
        {
            CurrentCulture = cultureInfo;
        }

        public LocalizationImplementation(string cultureInfo)
        {
            CurrentCulture = new CultureInfo(cultureInfo);
        }

        public string Delimiter { get; set; } = string.Empty;
        public IList<LanguageInfo> Languages { get; } = new List<LanguageInfo>();
        public CultureInfo CurrentCulture { get; set; }
        public bool LeaveUnusedLanguages { get; set; } = true;

        public void LoadLanguagesFromFile(string path)
        {
            var content = FileLoad(path);
            LoadLanguages(content);
        }

        public void LoadLanguagesFromString(string content)
        {
            LoadLanguages(content);
        }

        public string this[string key]
        {
            get { return GetValue(key, CurrentCulture); }
        }

        public IEnumerable<string> this[params string[] key]
        {
            get
            {
                foreach (var item in key)
                {
                    yield return  GetValue(item, CurrentCulture);
                }
                
            }
        }

        public dynamic Dynamic
        {
            get
            {
                IDictionary<string, object> dictionary = GetCurrentCultureDictionary(CurrentCulture).ToDictionary(pair => pair.Key, pair => (object)pair.Value);
                return ToExpandoObject(dictionary);
            }
        }

        private string GetValue(string key, CultureInfo culture)
        {
            var langDictionary = GetCurrentCultureDictionary(culture);
            string message;

            if (langDictionary.TryGetValue(key, out message))
            {
                return message;
            }

            if (CurrentCulture != defaultCulture && defaultCulture != null)
            {
                return GetValue(key, defaultCulture);
            }

            return string.Empty;
        }

        private void LoadLanguages(string content)
        {
            var reader = string.IsNullOrEmpty(Delimiter) ? new CsvFileReader(content) : new CsvFileReader(content, Delimiter[0]);

            Languages.Clear();
            MakeDictionary(reader);
            FillDictionary(reader);
            ClearDictionary();
        }

        private void MakeDictionary(CsvFileReader reader)
        {
            languageDictionary = new Dictionary<CultureInfo, Dictionary<string, string>>();
            foreach (var header in reader.ReadHeader())
            {
                if (!string.IsNullOrEmpty(header))
                {
                    if (header.ToLowerInvariant().StartsWith(DefaultDelimeter))
                    {
                        var defaultString = header.Split(new[] {DefaultDelimeter}, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

                        if (!string.IsNullOrEmpty(defaultString))
                        {
                            defaultCulture = new CultureInfo(defaultString);
                        }
                    }
                    else
                    {
                        AddLanguageInfo(header);
                        languageDictionary.Add(new CultureInfo(header), new Dictionary<string, string>());
                    }
                }
            }
        }

        private void AddLanguageInfo(string cultureCode)
        {
            var cultureInfo = new CultureInfo(cultureCode);
            var info = new LanguageInfo
            {
                DisplayName = cultureInfo.DisplayName,
                EnglishName = cultureInfo.EnglishName,
                NativeName = cultureInfo.NativeName,
                Name = cultureInfo.Name,
                TwoLetterName = cultureInfo.TwoLetterISOLanguageName
            };
            Languages.Add(info);
        }

        private void ClearDictionary()
        {
            if (!LeaveUnusedLanguages)
            {
                var items = languageDictionary.Where(w => w.Key != CurrentCulture).ToList();
                foreach (var item in items)
                {
                    languageDictionary.Remove(item.Key);
                }
            }
        }

        private void FillDictionary(CsvFileReader reader)
        {
            foreach (var row in reader.ReadRows())
            {
                var count = 1;
                foreach (var item in languageDictionary)
                {
                    try
                    {
                        item.Value.Add(row[0], row[count]);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        //hide exception
                    }

                    count++;
                }
            }
        }

        private Dictionary<string, string> GetCurrentCultureDictionary(CultureInfo culture)
        {
            Dictionary<string, string> currentDictionary;
            if (!languageDictionary.TryGetValue(culture, out currentDictionary))
            {
                if (culture.Name.Length == 2)
                {
                    foreach (var item in languageDictionary)
                    {
                        if (item.Key.TwoLetterISOLanguageName == culture.Name)
                        {
                            return item.Value;
                        }
                    }
                }

                if (culture.Name.Length > 2)
                {
                    foreach (var item in languageDictionary)
                    {
                        if (item.Key.TwoLetterISOLanguageName == culture.TwoLetterISOLanguageName)
                        {
                            return item.Value;
                        }
                    }
                }

                if (defaultCulture != null)
                {
                    if (languageDictionary.TryGetValue(defaultCulture, out currentDictionary))
                    {
                        return currentDictionary;
                    }
                }

                return new Dictionary<string, string>();
            }

            return currentDictionary;
        }

        private ExpandoObject ToExpandoObject(IDictionary<string, object> dictionary)
        {
            var expando = new ExpandoObject();
            var eoCol = (ICollection<KeyValuePair<string, object>>)expando;
            foreach (var kvp in dictionary)
            {
                eoCol.Add(kvp);
            }
            return expando;
        }
    }
}
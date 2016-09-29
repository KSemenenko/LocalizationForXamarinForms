using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using Localization.Shared.Parsers;
using NUnit.Framework;
using Plugin.Localization;

namespace Localization.Tests
{
    [TestFixture]
    public class TestClass
    {
        private readonly string csv = @"a1;a2;a3
b1;""b2; b5""; b3
c1; c2; c3";

        private readonly string csvLang = @"default:en;en;ru
val1;v1_en; v1_ru
val2;v2_en; v2_ru";

        private readonly string[] sampleList = {"a1", "a2", "a3", "b1", "b2; b5", "b3", "c1", "c2", "c3"};

        [Test]
        public void ReadAllRows()
        {
            using(var reader = new CsvFileReader(csv, ';'))
            {
                var resultList = new List<string>();
                foreach(var row in reader.ReadAllRows())
                {
                    foreach(var s in row)
                    {
                        resultList.Add(s);
                    }
                }

                resultList.ToArray().ShouldBeEquivalentTo(sampleList);
            }
        }

        [Test]
        public void ReadHeader()
        {
            var reader = new CsvFileReader(csv);
            var header = reader.ReadHeader();

            header.ToArray().ShouldBeEquivalentTo(sampleList.Take(3));
        }

        [Test]
        public void ReadRows()
        {
            using(var reader = new CsvFileReader(csv, ';'))
            {
                var resultList = new List<string>();
                foreach(var row in reader.ReadRows())
                {
                    foreach(var s in row)
                    {
                        resultList.Add(s);
                    }
                }

                resultList.ToArray().ShouldBeEquivalentTo(sampleList.Skip(3));
            }
        }

        [Test]
        public void LoclaizeImplement()
        {
            var loc = new LocalizationImplementation();
            loc.LoadLanguagesFromFile(csvLang);

            loc.CurrentCulture = new CultureInfo("en");
            loc["val1"].ShouldBeEquivalentTo("v1_en");

            loc.CurrentCulture = new CultureInfo("ru");
            loc["val1"].ShouldBeEquivalentTo("v1_ru");

            loc.CurrentCulture = new CultureInfo("en");
            loc["val2"].ShouldBeEquivalentTo("v2_en");

            loc.CurrentCulture = new CultureInfo("ru");
            loc["val2"].ShouldBeEquivalentTo("v2_ru");
        }

        [Test]
        public void LoclaizeImplementNewLanguage()
        {
            var loc = new LocalizationImplementation();
            loc.LoadLanguagesFromFile(csvLang);

            loc.CurrentCulture = new CultureInfo("en");
            loc["val1"].ShouldBeEquivalentTo("v1_en");

            loc.CurrentCulture = new CultureInfo("fr");
            loc["val1"].ShouldBeEquivalentTo("v1_en");
        }

        [Test]
        public void LoclaizeImplementEmptyFile()
        {
            var loc = new LocalizationImplementation();
            loc.LoadLanguagesFromFile(string.Empty);

            loc.CurrentCulture = new CultureInfo("en");
            loc["val1"].ShouldBeEquivalentTo(string.Empty);

            loc.CurrentCulture = new CultureInfo("fr");
            loc["val1"].ShouldBeEquivalentTo(string.Empty);
        }

        [Test]
        public void DynamicLoclaizeImplementEmptyFile()
        {
            var loc = new LocalizationImplementation();
            loc.LoadLanguagesFromFile(string.Empty);

            Action act = () => { var l = loc.Dynamic.val1; };

            act.ShouldThrow<Exception>();
        }

        [Test]
        public void CultureTest()
        {
            var c0 = new CultureInfo("en");
            var c1 = new CultureInfo("ru");

            var c02 = new CultureInfo("en-US");
            //var c1 = new CultureInfo("ru-RU");
            //var c2 = new CultureInfo("ar-AE");
            //var c3 = new CultureInfo("it-IT");
            //var c4 = new CultureInfo("es-ES");
            //var c5 = new CultureInfo("tr-TR");

            var loc = new LocalizationImplementation();
            loc.LoadLanguagesFromFile(csvLang);

            loc.Languages.Count.ShouldBeEquivalentTo(2);

            loc.Languages[0].DisplayName.ShouldBeEquivalentTo(c0.DisplayName);
            loc.Languages[0].EnglishName.ShouldBeEquivalentTo(c0.EnglishName);
            loc.Languages[0].NativeName.ShouldBeEquivalentTo(c0.NativeName);
            loc.Languages[0].Name.ShouldBeEquivalentTo(c0.Name);
            loc.Languages[0].TwoLetterName.ShouldBeEquivalentTo(c0.TwoLetterISOLanguageName);

            loc.Languages[1].DisplayName.ShouldBeEquivalentTo(c1.DisplayName);
            loc.Languages[1].EnglishName.ShouldBeEquivalentTo(c1.EnglishName);
            loc.Languages[1].NativeName.ShouldBeEquivalentTo(c1.NativeName);
            loc.Languages[1].Name.ShouldBeEquivalentTo(c1.Name);
            loc.Languages[0].Name.ShouldBeEquivalentTo(c0.TwoLetterISOLanguageName);
        }

        [Test]
        public void ShortCultureTest()
        {
            string shortLang = @"default:en;en;ru
val1;v1_en; v1_ru
val2;v2_en; v2_ru";

            var c1 = new CultureInfo("ru-RU");
            var c2 = new CultureInfo("en-US");
            var c3 = new CultureInfo("it-IT");

            var loc = new LocalizationImplementation();
            loc.LoadLanguagesFromFile(shortLang);

            loc.CurrentCulture = c1;
            loc["val1"].ShouldBeEquivalentTo("v1_ru");
            loc["val2"].ShouldBeEquivalentTo("v2_ru");

            loc.CurrentCulture = c2;
            loc["val1"].ShouldBeEquivalentTo("v1_en");
            loc["val2"].ShouldBeEquivalentTo("v2_en");

            loc.CurrentCulture = c3;
            loc["val1"].ShouldBeEquivalentTo("v1_en");
            loc["val2"].ShouldBeEquivalentTo("v2_en");
        }

        [Test]
        public void LongCultureTest()
        {
            string slongLang = @"default:en-US;en-US;ru-RU
val1;v1_en; v1_ru
val2;v2_en; v2_ru";

            var c1 = new CultureInfo("ru");
            var c2 = new CultureInfo("en");
            var c3 = new CultureInfo("it");

            var loc = new LocalizationImplementation();
            loc.LoadLanguagesFromFile(slongLang);

            loc.CurrentCulture = c1;
            loc["val1"].ShouldBeEquivalentTo("v1_ru");
            loc["val2"].ShouldBeEquivalentTo("v2_ru");

            loc.CurrentCulture = c2;
            loc["val1"].ShouldBeEquivalentTo("v1_en");
            loc["val2"].ShouldBeEquivalentTo("v2_en");


            loc.CurrentCulture = c3;
            loc["val1"].ShouldBeEquivalentTo("v1_en");
            loc["val2"].ShouldBeEquivalentTo("v2_en");
        }

        [Test]
        public void DynamicLoclaizeImplement()
        {
            var loc = new LocalizationImplementation();
            loc.LoadLanguagesFromFile(csvLang);

            loc.CurrentCulture = new CultureInfo("en");
            (loc.Dynamic.val1 as object).ShouldBeEquivalentTo("v1_en");

            loc.CurrentCulture = new CultureInfo("ru");
            (loc.Dynamic.val1 as object).ShouldBeEquivalentTo("v1_ru");

            loc.CurrentCulture = new CultureInfo("en");
            (loc.Dynamic.val2 as object).ShouldBeEquivalentTo("v2_en");

            loc.CurrentCulture = new CultureInfo("ru");
            (loc.Dynamic.val2 as object).ShouldBeEquivalentTo("v2_ru");
        }
    }
}
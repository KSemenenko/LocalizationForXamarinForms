using System;
using System.Collections.Generic;
using System.Configuration;
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

            loc.CurrentCulture = "en";
            loc["val1"].ShouldBeEquivalentTo("v1_en");

            loc.CurrentCulture = "ru";
            loc["val1"].ShouldBeEquivalentTo("v1_ru");

            loc.CurrentCulture = "en";
            loc["val2"].ShouldBeEquivalentTo("v2_en");

            loc.CurrentCulture = "ru";
            loc["val2"].ShouldBeEquivalentTo("v2_ru");
        }

        [Test]
        public void LoclaizeImplementNewLanguage()
        {
            var loc = new LocalizationImplementation();
            loc.LoadLanguagesFromFile(csvLang);

            loc.CurrentCulture = "en";
            loc["val1"].ShouldBeEquivalentTo("v1_en");

            loc.CurrentCulture = "fr";
            loc["val1"].ShouldBeEquivalentTo("v1_en");
        }

        [Test]
        public void LoclaizeImplementEmptyFile()
        {
            var loc = new LocalizationImplementation();
            loc.LoadLanguagesFromFile(string.Empty);

            loc.CurrentCulture = "en";
            loc["val1"].ShouldBeEquivalentTo(string.Empty);

            loc.CurrentCulture = "fr";
            loc["val1"].ShouldBeEquivalentTo(string.Empty);
        }

        [Test]
        public void DynamicLoclaizeImplementEmptyFile()
        {
            var loc = new LocalizationImplementation();
            loc.LoadLanguagesFromFile(string.Empty);

            Action act = () =>
            {
                var l = loc.Dynamic.val1;
            };

            act.ShouldThrow<Exception>();

        }

        [Test]
        public void DynamicLoclaizeImplement()
        {
            var loc = new LocalizationImplementation();
            loc.LoadLanguagesFromFile(csvLang);

            loc.CurrentCulture = "en";
            (loc.Dynamic.val1 as object).ShouldBeEquivalentTo("v1_en");

            loc.CurrentCulture = "ru";
            (loc.Dynamic.val1 as object).ShouldBeEquivalentTo("v1_ru");

            loc.CurrentCulture = "en";
            (loc.Dynamic.val2 as object).ShouldBeEquivalentTo("v2_en");

            loc.CurrentCulture = "ru";
            (loc.Dynamic.val2 as object).ShouldBeEquivalentTo("v2_ru");
        }
    }
}
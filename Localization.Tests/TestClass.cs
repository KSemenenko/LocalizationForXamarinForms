using System;
using System.Collections.Generic;
using FluentAssertions;
using Localization.Shared.Parsers;
using NUnit.Framework;

namespace Localization.Tests
{
    [TestFixture]
    public class TestClass
    {
        private readonly string csv = @"a1;a2;a3b1;""b2; b5""; b3c1; c2; c3";

        private readonly string[] sampleList = {"a1", "a2", "a3", "b1", "b2; b5", "b3", "c1", "c2", "c3"};

        [Test]
        public void TestMethod1()
        {
            using(var reader = new CsvFileReader(csv, ';'))
            {
                var resultList = new List<string>();
                foreach(var row in reader.ReadRow())
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
        public void TestMethod2()
        {
        }
    }
}
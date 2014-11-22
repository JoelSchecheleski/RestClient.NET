using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkaCahToa.Rest.Extensions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SkaCahToa.Rest.Tests.Extensions
{
	[ExcludeFromCodeCoverage]
	[TestClass]
    public class DictionaryExTests
    {
        [TestMethod]
        public void TestToQueryString()
        {
            Dictionary<string, string> testData = new Dictionary<string, string>();
            string expectedResult = "";

            Assert.AreEqual(expectedResult, testData.ToQueryString());

            testData.Add("param", "Value");

            expectedResult = "param=Value";

            Assert.AreEqual(expectedResult, testData.ToQueryString());

            testData.Add("2", "v2");

            expectedResult = "2=v2&param=Value";

            Assert.AreEqual(expectedResult, testData.ToQueryString());
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkaCahToa.Rest.Models;
using SkaCahToa.Rest.Models.Attributes;
using System;

namespace SkaCahToa.Rest.Tests.Models
{
    [TestClass]
    public class RestRequestTests
    {
        [SegmentDef(1, UrlDefinitionDataTypes.Static, "george")]
        [SegmentDef(3, UrlDefinitionDataTypes.Static, "bluth")]
        [SegmentDef(2, UrlDefinitionDataTypes.Data, "MiddleName")]
        [ParameterDef("show", UrlDefinitionDataTypes.Static, "Arrested Development")]
        [ParameterDef("TimeStamp", UrlDefinitionDataTypes.Data, "CurrentTimeStamp")]
        private class TestRestRequestUrl : RestRequest
        {
            public string CurrentTimeStamp { get { return DateTime.MinValue.ToString(); } }

            public string MiddleName { get; set; }
        }

        [TestMethod]
        public void GetModelURLTests()
        {
            TestRestRequestUrl trru = new TestRestRequestUrl()
            {
                MiddleName = "michael"
            };

            string expected = "http://www.google.com/george/michael/bluth?show=Arrested%20Development&TimeStamp=1%2F1%2F0001%2012%3A00%3A00%20AM";
            string actual = trru.GetModelURL("http://www.google.com/").ToString();

            Assert.AreEqual<string>(expected, actual);
        }
    }
}
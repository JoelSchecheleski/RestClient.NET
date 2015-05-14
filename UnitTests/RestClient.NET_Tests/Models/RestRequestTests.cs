using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkaCahToa.Rest.Models;
using SkaCahToa.Rest.Models.Attributes;
using SkaCahToa.Rest.Web;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace SkaCahToa.Rest.Tests.Models
{
	[ExcludeFromCodeCoverage]
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
            internal override HttpMethod GetHttpMethodType()
            {
                throw new NotImplementedException();
            }

            public string CurrentTimeStamp { get { return DateTime.MinValue.ToString(); } }

			public string MiddleName { get; set; }
		}

		[SegmentDef(1, UrlDefinitionDataTypes.Static, "george")]
		[SegmentDef(3, UrlDefinitionDataTypes.Static, "bluth")]
		[SegmentDef(2, UrlDefinitionDataTypes.Data, "MiddleName")]
		[ParameterDef("show", UrlDefinitionDataTypes.Static, "Arrested Development")]
		[ParameterDef("TimeStamp", UrlDefinitionDataTypes.Data, "CurrentTimeStamp")]
		private class TestRestRequestUrlException : RestRequest
        {
            internal override HttpMethod GetHttpMethodType()
            {
                throw new NotImplementedException();
            }
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
			string actual = new RestUrlBuilder("http://www.google.com/", trru).ToString();

			Assert.AreEqual<string>(expected, actual);
		}
	}
}
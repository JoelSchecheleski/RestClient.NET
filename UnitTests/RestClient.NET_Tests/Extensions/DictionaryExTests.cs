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

		[TestMethod]
		public void TestAddSafe()
		{
			Dictionary<int, string> testData = new Dictionary<int, string>();

			Assert.AreEqual<int>(0, testData.Count);
			Assert.AreEqual<bool>(true, testData.AddSafe(1, "test"));
			Assert.AreEqual<int>(1, testData.Count);
			Assert.AreEqual<bool>(true, testData.AddSafe(2, "test"));
			Assert.AreEqual<int>(2, testData.Count);
			Assert.AreEqual<bool>(false, testData.AddSafe(2, "test"));
			Assert.AreEqual<int>(2, testData.Count);
			Assert.AreEqual<bool>(false, testData.AddSafe(1, "test"));
			Assert.AreEqual<int>(2, testData.Count);
			Assert.AreEqual<bool>(true, testData.AddSafe(3, "test"));
			Assert.AreEqual<int>(3, testData.Count);
		}
	}
}
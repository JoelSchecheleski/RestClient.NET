using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkaCahToa.Rest.Models;
using SkaCahToa.Rest.Serializers;
using System.Diagnostics.CodeAnalysis;

namespace SkaCahToa.Rest.Tests.Serializers
{
	[ExcludeFromCodeCoverage]
	[TestClass]
	public class JsonRestDataSerializerTests
	{
		private class JSONTestObject : RestRequest
		{
			public string field1 { get; set; }

			public int field2 { get; set; }

			public bool field3 { get; set; }
		}

		private class JSONTestObject2 : RestResult
		{
			public string field1 { get; set; }

			public int field2 { get; set; }

			public bool field3 { get; set; }
		}

		[TestMethod]
		public void TestToDataType()
		{
			JSONTestObject test = new JSONTestObject()
			{
				field1 = "Testset",
				field2 = 4,
				field3 = false
			};

			IRestDataSerializer serializer = new JsonRestDataSerializer();

			string expected = "{\"field1\":\"Testset\",\"field2\":4,\"field3\":false}";
			Assert.AreEqual<string>(expected, serializer.ToDataType<JSONTestObject>(test));
		}

		[TestMethod]
		public void TestFromDataType()
		{
			string test = "{\"field1\":\"Testset\",\"field2\":4,\"field3\":false}";
			JSONTestObject2 expected = new JSONTestObject2()
			{
				field1 = "Testset",
				field2 = 4,
				field3 = false
			};

			IRestDataSerializer serializer = new JsonRestDataSerializer();
			JSONTestObject2 actual = serializer.FromDataType<JSONTestObject2>(test);

			Assert.AreEqual<string>(expected.field1, actual.field1);
			Assert.AreEqual<int>(expected.field2, actual.field2);
			Assert.AreEqual<bool>(expected.field3, actual.field3);
		}
	}
}
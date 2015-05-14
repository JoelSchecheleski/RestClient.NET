using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkaCahToa.Rest.Models;
using SkaCahToa.Rest.Models.Attributes;
using SkaCahToa.Rest.Serializers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Runtime.Serialization;

namespace SkaCahToa.Rest.Tests.Serializers
{
	[ExcludeFromCodeCoverage]
	[TestClass]
	public class JsonRestDataSerializerTests
	{
		[DataContract]
		private class JSONTestObject : RestRequest
        {
            internal override HttpMethod GetHttpMethodType()
            {
                throw new NotImplementedException();
            }

            [DataMember(Name = "field1")]
			public string field1 { get; set; }

			[DataMember(Name = "field2")]
			public int field2 { get; set; }

			[DataMember(Name = "field3")]
			public bool field3 { get; set; }
		}

		[DataContract]
		private class JSONTestObject2 : RestResult
		{
			[DataMember(Name = "field1")]
			public string field1 { get; set; }

			[DataMember(Name = "field2")]
			public int field2 { get; set; }

			[DataMember(Name = "field3")]
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
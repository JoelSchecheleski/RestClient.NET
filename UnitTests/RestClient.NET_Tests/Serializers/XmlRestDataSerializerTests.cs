using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkaCahToa.Rest.Exceptions;
using SkaCahToa.Rest.Models;
using SkaCahToa.Rest.Serializers;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SkaCahToa.Rest.Tests.Serializers
{
	[ExcludeFromCodeCoverage]
	[TestClass]
	public class XmlRestDataSerializerTests
	{
		[Serializable]
		public class XmlTestObject : RestRequest
		{
			public string field1 { get; set; }

			public int field2 { get; set; }

			public bool field3 { get; set; }
		}

		[Serializable]
		public class XmlTestObject2 : RestResult
		{
			public string field1 { get; set; }

			public int field2 { get; set; }

			public bool field3 { get; set; }
		}

		[TestMethod]
		public void TestToDataType()
		{
			IRestDataSerializer serializer = new XmlRestDataSerializer();

			XmlTestObject obj = new XmlTestObject()
			{
				field1 = "1",
				field2 = 2,
				field3 = true
			};

			string result = serializer.ToDataType<XmlTestObject>(obj);

			Assert.AreEqual<string>("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<XmlTestObject xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <field1>1</field1>\r\n  <field2>2</field2>\r\n  <field3>true</field3>\r\n</XmlTestObject>", result);
		}

		[TestMethod]
		public void TestFromDataType()
		{
			IRestDataSerializer serializer = new XmlRestDataSerializer();

			XmlTestObject2 actual = serializer.FromDataType<XmlTestObject2>("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<XmlTestObject2 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <field1>1</field1>\r\n  <field2>2</field2>\r\n  <field3>true</field3>\r\n</XmlTestObject2>");

			Assert.AreEqual<string>("1", actual.field1);
			Assert.AreEqual<int>(2, actual.field2);
			Assert.AreEqual<bool>(true, actual.field3);
		}
	}
}
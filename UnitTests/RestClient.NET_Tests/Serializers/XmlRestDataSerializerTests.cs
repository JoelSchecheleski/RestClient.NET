using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkaCahToa.Rest.Exceptions;
using SkaCahToa.Rest.Models;
using SkaCahToa.Rest.Serializers;

namespace SkaCahToa.Rest.Tests.Serializers
{
    [TestClass]
    public class XmlRestDataSerializerTests
    {
        [TestMethod]
        [ExpectedException(typeof(RestClientDotNetException), "XML isn't supported yet.")]
        public void TestToDataType()
        {
            IRestDataSerializer serializer = new XmlRestDataSerializer();
            serializer.ToDataType<RestRequest>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(RestClientDotNetException), "XML isn't supported yet.")]
        public void TestFromDataType()
        {
            IRestDataSerializer serializer = new XmlRestDataSerializer();
            RestResult actual = serializer.FromDataType<RestResult>(null);
        }
    }
}
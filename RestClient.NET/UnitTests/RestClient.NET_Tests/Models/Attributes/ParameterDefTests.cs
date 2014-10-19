using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkaCahToa.Rest.Models.Attributes;

namespace SkaCahToa.Rest.Tests.Models.Attributes
{
    [TestClass]
    public class ParameterDefTests
    {
        [TestMethod]
        public void ConstructorTests()
        {
            ParameterDef pd = new ParameterDef("key", UrlDefinitionDataTypes.Data, "value");

            Assert.AreEqual<string>(pd.Key, "key");
            Assert.AreEqual<string>(pd.Type.ToString(), UrlDefinitionDataTypes.Data.ToString());
            Assert.AreEqual<string>(pd.Value, "value");
        }
    }
}

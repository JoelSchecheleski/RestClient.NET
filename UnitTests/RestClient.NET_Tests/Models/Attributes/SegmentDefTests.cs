using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkaCahToa.Rest.Models.Attributes;

namespace SkaCahToa.Rest.Tests.Models.Attributes
{
    [TestClass]
    public class SegmentDefTests
    {
        [TestMethod]
        public void ConstructorTests()
        {
            SegmentDef pd = new SegmentDef(1, UrlDefinitionDataTypes.Data, "value");

            Assert.AreEqual<int>(pd.Order, 1);
            Assert.AreEqual<string>(pd.Type.ToString(), UrlDefinitionDataTypes.Data.ToString());
            Assert.AreEqual<string>(pd.Value, "value");
        }
    }
}
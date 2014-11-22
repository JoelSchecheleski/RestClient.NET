using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkaCahToa.Rest.Web;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SkaCahToa.Rest.Tests.Web
{
	[ExcludeFromCodeCoverage]
	[TestClass]
    public class RestUrlTests
    {
        [TestMethod]
        public void ToStringTests()
        {
            Dictionary<int, string> segments = new Dictionary<int, string>();
            segments.Add(1, "Api");

            Dictionary<string, string> qstring = new Dictionary<string, string>();
            qstring.Add("sort", "newest");

            RestUrl actual = new RestUrl("http://www.slashdot.org", segments, qstring);

            actual.AddSegment(2, "news");
            actual.AddQueryStringParam("clientid", "reptar");

            string expected = "http://www.slashdot.org/Api/news?clientid=reptar&sort=newest";

            Assert.AreEqual<string>(expected, actual.ToString());
        }
    }
}
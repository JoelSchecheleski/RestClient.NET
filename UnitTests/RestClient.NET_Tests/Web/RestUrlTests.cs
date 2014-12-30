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
			segments.Add(2, "news");

			Dictionary<string, string> qstring = new Dictionary<string, string>();
			qstring.Add("sort", "newest");
			qstring.Add("clientid", "reptar");

			RestUrlBuilder actual = new RestUrlBuilder("http://www.slashdot.org", segments, qstring);

			string expected = "http://www.slashdot.org/Api/news?clientid=reptar&sort=newest";

			Assert.AreEqual<string>(expected, actual.ToString());
		}
	}
}
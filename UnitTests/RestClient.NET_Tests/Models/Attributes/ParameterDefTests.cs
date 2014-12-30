using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkaCahToa.Rest.Models.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace SkaCahToa.Rest.Tests.Models.Attributes
{
	[ExcludeFromCodeCoverage]
	[TestClass]
	public class ParameterDefTests
	{
		[TestMethod]
		public void ConstructorTests()
		{
			ParameterDef pd = new ParameterDef("key", UrlDefinitionDataTypes.Data, "value");

			Assert.AreEqual<string>(pd.Name, "key");
			Assert.AreEqual<string>(pd.Type.ToString(), UrlDefinitionDataTypes.Data.ToString());
			Assert.AreEqual<string>(pd.Value, "value");
		}
	}
}
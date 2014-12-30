using System;

namespace SkaCahToa.Rest.Models.Attributes
{
	/// <summary>
	/// Attribute that defines a querystring parameter for a model
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class ParameterDef : UrlDefinitionBase
	{
		#region Properties

		/// <summary>
		/// Querystring Parameter Key Name
		/// </summary>
		public string Name { get; set; }

		#endregion Properties

		public ParameterDef(string name, UrlDefinitionDataTypes type, string value)
			: base(type, value)
		{
			Name = name;
		}
	}
}
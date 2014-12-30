using System;

namespace SkaCahToa.Rest.Models.Attributes
{
	/// <summary>
	/// Class Attribute that defines a Url Segment for a model
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class SegmentDef : UrlDefinitionBase
	{
		#region Properties

		public int Order { get; set; }

		#endregion Properties

		public SegmentDef(int order, UrlDefinitionDataTypes type, string value)
			: base(type, value)
		{
			Order = order;
		}
	}
}
using System;

namespace SkaCahToa.Rest.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class SegmentDef : UrlDefinitionBase
    {
        public int Order { get; set; }

        public SegmentDef(int order, UrlDefinitionDataTypes type, string value) : base(type, value)
        {
            Order = order;
        }
    }
}
using System;

namespace SkaCahToa.Rest.Models.Attributes
{
    public enum UrlDefinitionDataTypes
    {
        Static,
        Data
    }

    public abstract class UrlDefinitionBase : Attribute
    {
        public UrlDefinitionDataTypes Type { get; set; }

        public string Value { get; set; }

        public UrlDefinitionBase(UrlDefinitionDataTypes type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
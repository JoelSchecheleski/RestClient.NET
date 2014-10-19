using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

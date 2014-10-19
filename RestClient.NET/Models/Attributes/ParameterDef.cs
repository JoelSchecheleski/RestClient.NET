using System;

namespace SkaCahToa.Rest.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ParameterDef : UrlDefinitionBase
    {
        public string Key { get; set; }

        public ParameterDef(string key, UrlDefinitionDataTypes type, string value) : base(type, value)
        {
            Key = key;
        }
    }
}
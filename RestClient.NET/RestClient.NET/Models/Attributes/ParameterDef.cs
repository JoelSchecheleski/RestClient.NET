using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkaCahToa.Rest.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ParameterDef : UrlDefinitionBase
    {
        public string Key { get; set; }

        public ParameterDef(string key, UrlDefinitionDataTypes type, string value) : base(type, value)
        {
            Key = key;
        }
    }
}

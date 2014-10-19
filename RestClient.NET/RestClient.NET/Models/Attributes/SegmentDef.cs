using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkaCahToa.Rest.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SegmentDef : UrlDefinitionBase
    {
        public int Order { get; set; }

        public SegmentDef(int order, UrlDefinitionDataTypes type, string value) : base(type, value)
        {
            Order = order;
        }
    }
}

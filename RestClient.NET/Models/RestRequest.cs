using SkaCahToa.Rest.Models.Attributes;
using SkaCahToa.Rest.Web;
using System.Linq;
using System.Reflection;

namespace SkaCahToa.Rest.Models
{
    public class RestRequest
    {
        public RestUrl GetModelURL(string baseUrl)
        {
            RestUrl url = new RestUrl(baseUrl);
            TypeInfo ti = GetType().GetTypeInfo();

            foreach (UrlDefinitionBase def in ti.GetCustomAttributes()
                .Where(a => a is UrlDefinitionBase)
                .Select(a => (UrlDefinitionBase)a))
            {
                switch (def.Type)
                {
                    case UrlDefinitionDataTypes.Static:
                        if (def is SegmentDef)
                            url.AddSegment(((SegmentDef)def).Order, ((SegmentDef)def).Value);
                        else if (def is ParameterDef)
                            url.AddQueryStringParam(((ParameterDef)def).Key, ((ParameterDef)def).Value);
                        break;

                    case UrlDefinitionDataTypes.Data:
                        string value = GetType()
                                .GetRuntimeProperties()
                                .Single(s => s.Name == def.Value)
                                .GetValue(this)
                                .ToString();
                        if (def is SegmentDef)
                            url.AddSegment(((SegmentDef)def).Order, value);
                        else if (def is ParameterDef)
                            url.AddQueryStringParam(((ParameterDef)def).Key, value);
                        break;

                    default:
                        throw new Exceptions.RestClientDotNetException("Segment Type Not Supported.");
                }
            }

            return url;
        }
    }
}
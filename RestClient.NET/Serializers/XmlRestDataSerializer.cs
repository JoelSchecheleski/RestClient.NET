using Newtonsoft.Json;
using SkaCahToa.Rest.Exceptions;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace SkaCahToa.Rest.Serializers
{
    public class XmlRestDataSerializer : JsonRestDataSerializer
    {
        public override string ToDataType<RestRequestType>(RestRequestType model)
        {
            throw new RestClientDotNetException("XML isn't supported yet.");
        }

        public override RestResultType FromDataType<RestResultType>(string data)
        {
            throw new RestClientDotNetException("XML isn't supported yet.");
        }

        private string XmlToString(XDocument doc)
        {
            StringBuilder builder = new StringBuilder();
            using (TextWriter writer = new StringWriter(builder))
            {
                doc.Save(writer);
            }
            return builder.ToString();
        }
    }
}
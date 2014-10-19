using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace SkaCahToa.Rest.Serializers
{
    public class XmlRestDataSerializer : JsonRestDataSerializer
    {
        public override string ToDataType<RestRequestType>(RestRequestType model)
        {
            XDocument x = JsonConvert.DeserializeXNode(base.ToDataType<RestRequestType>(model));
            return XmlToString(x);
        }

        public override RestResultType FromDataType<RestResultType>(string data)
        {
            XDocument x = XDocument.Parse(data);
            return base.FromDataType<RestResultType>(JsonConvert.SerializeXNode(null));
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
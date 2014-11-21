using Newtonsoft.Json;
using SkaCahToa.Rest.Exceptions;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SkaCahToa.Rest.Serializers
{
    public class XmlRestDataSerializer : JsonRestDataSerializer
	{
		public override string ToDataType<RestRequestType>(RestRequestType model)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(RestRequestType));

			using (MemoryStream memStream = new MemoryStream())
			{
				serializer.Serialize(memStream, model);

				using (StreamReader streamReader = new StreamReader(memStream))
					return streamReader.ReadToEnd();
			}
        }

        public override RestResultType FromDataType<RestResultType>(string data)
        {
			XmlSerializer serializer = new XmlSerializer(typeof(RestResultType));

			return (RestResultType)serializer.Deserialize(new MemoryStream(Encoding.Unicode.GetBytes(data)));
		}
    }
}
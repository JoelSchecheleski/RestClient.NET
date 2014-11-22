using SkaCahToa.Rest.Models;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SkaCahToa.Rest.Serializers
{
    public class XmlRestDataSerializer : IRestDataSerializer
	{
		public virtual string ToDataType<RestRequestType>(RestRequestType model)
			where RestRequestType : RestRequest
		{
			XmlSerializer serializer = new XmlSerializer(typeof(RestRequestType));

			using (StringWriter stringWriter = new StringWriter())
			{
				serializer.Serialize(stringWriter, model);

				return stringWriter.ToString();
			}
        }

        public virtual RestResultType FromDataType<RestResultType>(string data)
			where RestResultType : RestResult
		{
			XmlSerializer serializer = new XmlSerializer(typeof(RestResultType));

			return (RestResultType)serializer.Deserialize(new MemoryStream(Encoding.Unicode.GetBytes(data)));
		}

		#region IDisposable

		public virtual void Dispose()
		{
		}

		#endregion IDisposable
	}
}
using SkaCahToa.Rest.Models;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SkaCahToa.Rest.Serializers
{
	/// <summary>
	/// RestClient.NET XML Serializer. It'll translate RestModels into XML strings.
	/// </summary>
	public sealed class XmlRestDataSerializer : IRestDataSerializer
	{
		#region IRestDataSerializer

		public string ToDataType<RestRequestType>(RestRequestType model)
			where RestRequestType : RestRequest
		{
			XmlSerializer serializer = new XmlSerializer(typeof(RestRequestType));

			using (StringWriter stringWriter = new StringWriter())
			{
				serializer.Serialize(stringWriter, model);

				return stringWriter.ToString();
			}
		}

		public RestResultType FromDataType<RestResultType>(string data)
			where RestResultType : RestResult
		{
			XmlSerializer serializer = new XmlSerializer(typeof(RestResultType));

			return (RestResultType)serializer.Deserialize(new MemoryStream(Encoding.Unicode.GetBytes(data)));
		}

		#endregion IRestDataSerializer

		#region IDisposable

		public void Dispose()
		{
		}

		#endregion IDisposable
	}
}
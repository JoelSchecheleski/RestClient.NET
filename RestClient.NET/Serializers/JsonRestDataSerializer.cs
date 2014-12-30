using SkaCahToa.Rest.Extensions;
using SkaCahToa.Rest.Models;
using SkaCahToa.Rest.Models.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SkaCahToa.Rest.Serializers
{
	/// <summary>
	/// RestClient.NET Json Serializer. It'll translate RestModels into JSON strings.
	/// </summary>
	public sealed class JsonRestDataSerializer : IRestDataSerializer
	{
		#region IRestDataSerializer

		public string ToDataType<RestRequestType>(RestRequestType model)
			where RestRequestType : RestRequest, new()
		{
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RestRequestType));

			using (MemoryStream stream = new MemoryStream())
			{
				serializer.WriteObject(stream, model);

				stream.Position = 0;

				using (StreamReader sr = new StreamReader(stream))
				{
					return sr.ReadToEnd();
				}
			}
		}

		public RestResultType FromDataType<RestResultType>(string data)
			where RestResultType : RestResult, new()
		{
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RestResultType));

			using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
				return (RestResultType)serializer.ReadObject(stream);
		}

		#endregion IRestDataSerializer

		#region IDisposable

		public void Dispose()
		{
		}

		#endregion IDisposable
	}
}
using Newtonsoft.Json;
using SkaCahToa.Rest.Models;

namespace SkaCahToa.Rest.Serializers
{
    public sealed class JsonRestDataSerializer : IRestDataSerializer
    {
		#region  IRestDataSerializer

		public string ToDataType<RestRequestType>(RestRequestType model)
            where RestRequestType : RestRequest
        {
            return JsonConvert.SerializeObject(model);
        }

        public RestResultType FromDataType<RestResultType>(string data)
            where RestResultType : RestResult
        {
            return JsonConvert.DeserializeObject<RestResultType>(data);
		}

		#endregion IRestDataSerializer

		#region IDisposable

		public void Dispose()
		{
		}

		#endregion IDisposable
	}
}
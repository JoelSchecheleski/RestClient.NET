using Newtonsoft.Json;
using SkaCahToa.Rest.Models;

namespace SkaCahToa.Rest.Serializers
{
    public class JsonRestDataSerializer : IRestDataSerializer
    {
        public virtual string ToDataType<RestRequestType>(RestRequestType model)
            where RestRequestType : RestRequest
        {
            return JsonConvert.SerializeObject(model);
        }

        public virtual RestResultType FromDataType<RestResultType>(string data)
            where RestResultType : RestResult
        {
            return JsonConvert.DeserializeObject<RestResultType>(data);
		}

		#region IDisposable

		public virtual void Dispose()
		{
		}

		#endregion IDisposable
	}
}
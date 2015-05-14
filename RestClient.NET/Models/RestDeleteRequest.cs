using System.Net.Http;
using System.Runtime.Serialization;

namespace SkaCahToa.Rest.Models
{
    [DataContract]
    public abstract class RestDeleteRequest : RestRequest
    {
        internal sealed override HttpMethod GetHttpMethodType()
        {
            return HttpMethod.Delete;
        }
    }
}

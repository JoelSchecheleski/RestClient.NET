using System.Net.Http;
using System.Runtime.Serialization;

namespace SkaCahToa.Rest.Models
{
	[DataContract]
	public abstract class RestRequest
	{
        internal abstract HttpMethod GetHttpMethodType();
    }
}
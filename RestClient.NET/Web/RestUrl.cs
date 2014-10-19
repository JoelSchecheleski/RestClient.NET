using SkaCahToa.Rest.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace SkaCahToa.Rest.Web
{
    public class RestUrl
    {
        protected string BaseUrl { get; set; }

        protected Dictionary<int, string> Segments { get; set; }

        protected Dictionary<string, string> QueryString { get; set; }

        public RestUrl(string baseUrl, Dictionary<int, string> segments = null, Dictionary<string, string> queryString = null)
        {
            BaseUrl = baseUrl.EndsWith("/") ? baseUrl : (baseUrl + "/");
            Segments = segments != null ? segments : new Dictionary<int, string>();
            QueryString = queryString != null ? queryString : new Dictionary<string, string>();
        }

        public void AddSegment(int order, string value)
        {
            if (!Segments.ContainsKey(order))
                Segments.Add(order, value);
        }

        public void AddQueryStringParam(string key, string value)
        {
            if (!QueryString.ContainsKey(key))
                QueryString.Add(key, value);
        }

        public override string ToString()
        {
            string url = BaseUrl;
            url += string.Join("/", Segments.OrderBy(s => s.Key).Select(s => s.Value.Trim('/')));
            return url + "?" + QueryString.ToQueryString();
        }
    }
}
using SkaCahToa.Rest.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace SkaCahToa.Rest.Web
{
    internal class RestUrl
    {
		#region Properties

		protected string BaseUrl { get; set; }

        protected Dictionary<int, string> Segments { get; set; }

        protected Dictionary<string, string> QueryString { get; set; }

		#endregion Properties

		public RestUrl(string baseUrl, Dictionary<int, string> segments = null, Dictionary<string, string> queryString = null)
        {
			BaseUrl = baseUrl.EndsWith("/") ? baseUrl : (baseUrl + "/");

			Segments = segments != null ? segments : new Dictionary<int, string>();

			QueryString = queryString != null ? queryString : new Dictionary<string, string>();
        }

        public void AddSegment(int order, string value)
        {
			Segments.AddSafe(order, value);
        }

        public void AddQueryStringParam(string key, string value)
        {
            QueryString.AddSafe(key, value);
        }

        public override string ToString()
        {
            string url = BaseUrl;

			if(Segments.Count > 0)
				url += string.Join("/", Segments.OrderBy(s => s.Key).Select(s => s.Value.Trim('/')));

			if(QueryString.Count > 0)
				url += "?" + QueryString.ToQueryString();

			return url;
        }
    }
}
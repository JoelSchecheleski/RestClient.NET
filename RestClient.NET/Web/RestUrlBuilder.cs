using SkaCahToa.Rest.Extensions;
using SkaCahToa.Rest.Models;
using SkaCahToa.Rest.Models.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SkaCahToa.Rest.Web
{
	/// <summary>
	/// Builds a valid request url using QueryString Parameters And Segments.
	/// </summary>
	internal class RestUrlBuilder
	{
		#region Properties

		protected string BaseUrl { get; set; }

		protected Dictionary<int, string> Segments { get; set; }

		protected Dictionary<string, string> QueryString { get; set; }

		#endregion Properties

		internal RestUrlBuilder(string baseUrl, Dictionary<int, string> segments, Dictionary<string, string> queryString)
		{
			BaseUrl = baseUrl.EndsWith("/") ? baseUrl : (baseUrl + "/");
			Segments = segments;
			QueryString = queryString;
		}

		internal RestUrlBuilder(string baseUrl, RestRequest request)
			: this(baseUrl, new Dictionary<int, string>(), new Dictionary<string, string>())
		{
			TypeInfo ti = request.GetType().GetTypeInfo();

			//Get Segment and QueryString data from Model
			foreach (UrlDefinitionBase def in ti.GetCustomAttributes()
				.Where(a => a is UrlDefinitionBase)
				.Select(a => (UrlDefinitionBase)a))
			{
				switch (def.Type)
				{
					case UrlDefinitionDataTypes.Static:
						if (def is SegmentDef)
							AddSegment(((SegmentDef)def).Order, ((SegmentDef)def).Value);
						else if (def is ParameterDef)
							AddQueryStringParam(((ParameterDef)def).Name, ((ParameterDef)def).Value);
						break;

					case UrlDefinitionDataTypes.Data:
						//Get value form model
						string value = request.GetType()
								.GetRuntimeProperties()
								.Single(s => s.Name == def.Value)
								.GetValue(request)
								.ToString();

						if (def is SegmentDef)
							AddSegment(((SegmentDef)def).Order, value);
						else if (def is ParameterDef)
							AddQueryStringParam(((ParameterDef)def).Name, value);
						break;

					default:
						throw new Exceptions.RestClientDotNetException("Segment Type Not Supported.");
				}
			}
		}

		/// <summary>
		/// Add Segment to Url
		/// </summary>
		/// <param name="order">Segment Position</param>
		/// <param name="value">Segment Value</param>
		protected void AddSegment(int order, string value)
		{
			Segments.AddSafe(order, value);
		}

		/// <summary>
		/// Add QueryString Parameter to Url
		/// </summary>
		/// <param name="key">QueryString Key</param>
		/// <param name="value">QueryString Value</param>
		protected void AddQueryStringParam(string key, string value)
		{
			QueryString.AddSafe(key, value);
		}

        #region Overrides

        /// <summary>
        /// Get Current Configured Url
        /// </summary>
        /// <returns>Current Rest Request Url</returns>
        public override string ToString()
		{
			string url = BaseUrl;

			//Add segments to base endpoint url
			if (Segments.Count > 0)
				url += string.Join("/", Segments.OrderBy(s => s.Key).Select(s => s.Value.Trim('/')));

			//build querystring
			if (QueryString.Count > 0)
				url += "?" + QueryString.ToQueryString();

			return url;
		}

        #endregion Overrides
    }
}
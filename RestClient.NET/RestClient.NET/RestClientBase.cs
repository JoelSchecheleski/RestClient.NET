using SkaCahToa.Rest.Extensions;
using SkaCahToa.Rest.Models;
using SkaCahToa.Rest.Models.Attributes;
using SkaCahToa.Rest.Serializers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SkaCahToa.Rest
{
    public abstract class RestClientBase
    {
        public enum DataTypes
        {
            JSON,
            XML
        }

        #region Constructors
        public RestClientBase(DataTypes dataType)
        {
            switch(dataType)
            {
                case DataTypes.JSON:
                    DataSerializer = new JsonRestDataSerializer();
                    break;
                case DataTypes.XML:
                    DataSerializer = new XmlRestDataSerializer();
                    break;
                default:
                    throw new Exceptions.RestHelperException("DataType Not Supported.");
            }
        }

        public RestClientBase(IRestDataSerializer serializer)
        {
            if (serializer == null)
                throw new Exceptions.RestHelperException("Serializer cannot be null");
            DataSerializer = serializer;
        }
        #endregion Constructors

        protected RestErrorResult LastError { get; private set; }

        protected IRestDataSerializer DataSerializer { get; private set; }

        protected abstract string Url { get; }

        protected abstract HttpClient SetupCreds(HttpClient hc);

        protected ResultType SendRequest<ResultType, RequestType, ErrorType>(RequestType data)
            where ResultType : RestResult
            where RequestType : RestRequest
            where ErrorType : RestErrorResult
        {
            Task<ResultType> task = SendRequestAsync<ResultType, RequestType, ErrorType>(data);

            return task.Result;
        }

        protected async Task<ResultType> SendRequestAsync<ResultType, RequestType, ErrorType>(RequestType data)
            where ResultType : RestResult
            where RequestType : RestRequest
            where ErrorType : RestErrorResult
        {
            HttpMethod type;

            if (data is RestPostRequest)
                type = HttpMethod.Post;
            else if (data is RestGetRequest)
                type = HttpMethod.Get;
            else
                throw new Exceptions.RestHelperException("Http Method Type Not Supported");

            return await SendRequestAsync<ResultType, RequestType, ErrorType>(
                BuildUrlFromModel(data),
                type,
                (type != HttpMethod.Get ? data : null)
            );
        }

        private async Task<ResultType> SendRequestAsync<ResultType, RequestType, ErrorType>(string url, HttpMethod methodType, RequestType data = null)
            where ResultType : RestResult
            where RequestType : RestRequest
            where ErrorType : RestErrorResult
        {
            HttpClient hc = new HttpClient();
            hc = SetupCreds(hc);

            HttpRequestMessage request = new HttpRequestMessage(methodType, url);

            if (data != null)
            {
                using (MemoryStream stream = new MemoryStream(
                    Encoding.Unicode.GetBytes(
                        DataSerializer.ToDataType<RequestType>(data))))
                {
                    request.Content = new StreamContent(stream);
                }
            }

            HttpResponseMessage response = await hc.SendAsync(request);
            
            return DataSerializer.FromDataType<ResultType>(await response.Content.ReadAsStringAsync());
        }

        private string BuildUrlFromModel(RestRequest model)
        {
            Dictionary<int, string> segments = new Dictionary<int, string>();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            TypeInfo ti = model.GetType().GetTypeInfo();
            string url = Url.EndsWith("/") ? Url : (Url + "/");

            foreach (UrlDefinitionBase def in ti.GetCustomAttributes()
                .Where(a => a is UrlDefinitionBase)
                .Select(a => (UrlDefinitionBase)a))
            {
                switch (def.Type)
                {
                    case UrlDefinitionDataTypes.Static:
                        if (def is SegmentDef)
                            segments.Add(((SegmentDef)def).Order, ((SegmentDef)def).Value);
                        else if (def is ParameterDef)
                            parms.Add(((ParameterDef)def).Key, ((ParameterDef)def).Value);
                        break;
                    case UrlDefinitionDataTypes.Data:
                        string value = model.GetType()
                                .GetRuntimeProperties()
                                .Single(s => s.Name == def.Value)
                                .GetValue(model)
                                .ToString();
                        if (def is SegmentDef)
                            segments.Add(((SegmentDef)def).Order, value);
                        else if (def is ParameterDef)
                            parms.Add(((ParameterDef)def).Key, value);
                        break;
                    default:
                        throw new Exceptions.RestHelperException("Segment Type Not Supported.");
                }
            }

            url += string.Join("/", segments.OrderBy(s => s.Key).Select(s => s.Value.Trim('/')));
            return url + "?" + parms.ToQueryString();
        }
    }
}

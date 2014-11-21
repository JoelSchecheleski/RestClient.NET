using SkaCahToa.Rest.Exceptions;
using SkaCahToa.Rest.Models;
using SkaCahToa.Rest.Serializers;
using SkaCahToa.Rest.Web;
using System;
using System.IO;
using System.Net.Http;
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
            switch (dataType)
            {
                case DataTypes.JSON:
                    DataSerializer = new JsonRestDataSerializer();
                    break;

                case DataTypes.XML:
                    DataSerializer = new XmlRestDataSerializer();
                    break;

                default:
                    throw new Exceptions.RestClientDotNetException("DataType Not Supported.");
            }
        }

        public RestClientBase(IRestDataSerializer serializer)
        {
            if (serializer == null)
                throw new Exceptions.RestClientDotNetException("Serializer cannot be null");
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
                throw new Exceptions.RestClientDotNetException("Http Method Type Not Supported");

            return await SendRequestAsync<ResultType, RequestType, ErrorType>(
                data.GetModelURL(Url),
                type,
                (type != HttpMethod.Get ? data : null)
            );
        }

        private async Task<ResultType> SendRequestAsync<ResultType, RequestType, ErrorType>(RestUrl url, HttpMethod methodType, RequestType data = null)
            where ResultType : RestResult
            where RequestType : RestRequest
            where ErrorType : RestErrorResult
        {
            HttpClient hc = new HttpClient();
            hc = SetupCreds(hc);

            HttpRequestMessage request = new HttpRequestMessage(methodType, url.ToString());

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

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return DataSerializer.FromDataType<ResultType>(await response.Content.ReadAsStringAsync());
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                try
                {
                    ErrorType error = DataSerializer.FromDataType<ErrorType>(await response.Content.ReadAsStringAsync());

                    throw new RestErrorResponseException(error, "request returned :" + response.StatusCode.ToString());
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
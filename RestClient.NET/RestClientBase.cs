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
    public abstract class RestClientBase : IDisposable
    {
        public enum DataTypes
        {
			NotImplemented,
            JSON,
            XML
        }

		#region Properties

		protected RestErrorResult LastError { get; private set; }

		protected IRestDataSerializer DataSerializer { get; private set; }

		protected HttpClient Client { get; set; }

		protected abstract string Url { get; }

		private bool Disposed { get; set; }

		#endregion Properties

		#region Constructors

		public RestClientBase(DataTypes dataType) : this()
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

        public RestClientBase(IRestDataSerializer serializer) : this()
        {
            if (serializer == null)
                throw new Exceptions.RestClientDotNetException("Serializer cannot be null");
            DataSerializer = serializer;
        }

		private RestClientBase()
		{
			Disposed = false;
			Client = null;
		}

		~RestClientBase()
		{
			Dispose(false);
		}

		#endregion Constructors

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
            ).ConfigureAwait(false);
        }

        private async Task<ResultType> SendRequestAsync<ResultType, RequestType, ErrorType>(RestUrl url, HttpMethod methodType, RequestType data = null)
            where ResultType : RestResult
            where RequestType : RestRequest
            where ErrorType : RestErrorResult
        {
			if (Client == null)
				Client = SetupCreds(new HttpClient());

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

            HttpResponseMessage response = await Client.SendAsync(request).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return DataSerializer.FromDataType<ResultType>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
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
                    LastError = DataSerializer.FromDataType<ErrorType>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                    throw new RestErrorResponseException(LastError, "request returned: " + response.StatusCode.ToString());
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

		#region IDisposable

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!Disposed)
			{
				if (disposing)
				{
				}

				if (Client != null)
					Client.Dispose();

				DataSerializer.Dispose();

				Disposed = true;
			}
		}

		#endregion IDisposable
	}
}
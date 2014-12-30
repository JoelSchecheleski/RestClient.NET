using SkaCahToa.Rest.Exceptions;
using SkaCahToa.Rest.Models;
using SkaCahToa.Rest.Serializers;
using SkaCahToa.Rest.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SkaCahToa.Rest
{
	/// <summary>
	/// Abstract Base RestClient Class
	/// </summary>
	public abstract class RestClientBase : IDisposable
	{
		public enum DataTypes
		{
			NotImplemented,
			JSON,
			XML
		}

		#region Properties

		/// <summary>
		/// Last Error Response
		/// </summary>
		protected RestErrorResult LastError { get; private set; }

		/// <summary>
		/// DataSerializer To Convert Model
		/// </summary>
		private IRestDataSerializer DataSerializer { get; set; }

		private HttpClient Client { get; set; }

		private bool Disposed { get; set; }

		#endregion Properties

		#region Constructors

		public RestClientBase(DataTypes dataType)
			: this()
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
			: this()
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

		#region Abstract Members

		/// <summary>
		/// Builds a HttpClient with Credentials Setups
		/// </summary>
		protected abstract HttpClient SetupConnection();

		/// <summary>
		/// Rest API Endpoint URL
		/// </summary>
		protected abstract string Url { get; }

		#endregion Abstract Members

		public ResultType SendRequest<ResultType, RequestType, ErrorType>(RequestType data)
			where ResultType : RestResult
			where RequestType : RestRequest
			where ErrorType : RestErrorResult
		{
			//Get Aysnc Send Request Task
			Task<ResultType> task = SendRequestAsync<ResultType, RequestType, ErrorType>(data);

			try
			{
				//Get Result.
				return task.Result;
			}
			catch (AggregateException e)
			{
				//If we catch a single Exception
				if (e.InnerExceptions.Count == 1)
				{
					//Throw it
					IEnumerator<Exception> exceptions = e.InnerExceptions.GetEnumerator();
					exceptions.MoveNext();
					throw exceptions.Current;
				}
				else
				{
					//Throw Aggregate if we have multiple
					throw;
				}
			}
		}

		public async Task<ResultType> SendRequestAsync<ResultType, RequestType, ErrorType>(RequestType data)
			where ResultType : RestResult
			where RequestType : RestRequest
			where ErrorType : RestErrorResult
		{
			HttpMethod type;

			//Get Method Type from Model
			if (data is RestPostRequest)
				type = HttpMethod.Post;
			else if (data is RestGetRequest)
				type = HttpMethod.Get;
			else
				throw new Exceptions.RestClientDotNetException("Http Method Type Not Supported");

			//Send Request
			return await SendRequestAsync<ResultType, RequestType, ErrorType>(
				new RestUrlBuilder(Url, data),
				type,
				(type != HttpMethod.Get ? data : null)
			).ConfigureAwait(false);
		}

		private async Task<ResultType> SendRequestAsync<ResultType, RequestType, ErrorType>(RestUrlBuilder url, HttpMethod methodType, RequestType data = null)
			where ResultType : RestResult
			where RequestType : RestRequest
			where ErrorType : RestErrorResult
		{
			if (Client == null)
				Client = SetupConnection();

			HttpRequestMessage request = new HttpRequestMessage(methodType, url.ToString());

			//Push data to request stream
			if (data != null)
			{
				using (MemoryStream stream = new MemoryStream(
					Encoding.Unicode.GetBytes(
						DataSerializer.ToDataType<RequestType>(data))))
				{
					request.Content = new StreamContent(stream);
				}
			}

			//Wait for request
			HttpResponseMessage response = await Client.SendAsync(request).ConfigureAwait(false);

			if (response.IsSuccessStatusCode)
			{
				try
				{
					//Get response as RestResponseModel
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
					//If result wasn't successful create error response model
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
			if (Disposed)
				return;

			if (Client != null)
				Client.Dispose();

			if (DataSerializer != null)
				DataSerializer.Dispose();

			Disposed = true;
		}

		#endregion IDisposable
	}
}
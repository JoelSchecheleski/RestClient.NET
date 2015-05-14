using SkaCahToa.Rest.Exceptions;
using SkaCahToa.Rest.Models;
using SkaCahToa.Rest.Serializers;
using SkaCahToa.Rest.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
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
			JSON,
			XML
		}

		#region Properties
        
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
			where ResultType : RestResult, new()
			where RequestType : RestRequest, new()
			where ErrorType : RestErrorResult, new()
		{
			//Get Aysnc Send Request Task
			Task<ResultType> task = SendRequestAsync<ResultType, RequestType, ErrorType>(data);

			try
			{
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
			catch (SerializationException)
			{
				throw new RestErrorResponseException(null, "Could not serialize response message.");
			}
		}

		public async Task<ResultType> SendRequestAsync<ResultType, RequestType, ErrorType>(RequestType data)
			where ResultType : RestResult, new()
			where RequestType : RestRequest, new()
			where ErrorType : RestErrorResult, new()
		{
            if (data == null)
                throw new ArgumentException("Request Model cannot be null", "data");

			if (Client == null)
				Client = SetupConnection();

			HttpRequestMessage request = new HttpRequestMessage(
                data.GetHttpMethodType(),
                (new RestUrlBuilder(Url, data)).ToString()
            );

			//Push data to request stream
			if (DoesHttpMethodTypeSupportBodyData(data.GetHttpMethodType()))
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
					return DataSerializer.FromDataType<ResultType>(
                        await response.Content.ReadAsStringAsync().ConfigureAwait(false)
                    );
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
					throw new RestErrorResponseException(
                        DataSerializer.FromDataType<ErrorType>(
                            await response.Content.ReadAsStringAsync().ConfigureAwait(false)
                        ),
                        "request returned: " + response.StatusCode.ToString()
                    );
				}
				catch (Exception)
				{
					throw;
				}
			}
		}
        
        protected virtual bool DoesHttpMethodTypeSupportBodyData(HttpMethod type)
        {
            return (type == HttpMethod.Post || type == HttpMethod.Put);
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
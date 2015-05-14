using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkaCahToa.Rest.Exceptions;
using SkaCahToa.Rest.Models;
using SkaCahToa.Rest.Serializers;
using SkaCahToa.Rest.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SkaCahToa.Rest.Tests
{
	[ExcludeFromCodeCoverage]
	[TestClass]
	public class RestClientBaseMockTest : RestClientBase
	{
		[TestMethod]
		public void TestSendRequestThrowExceptionIfNoModelMethodIsSelected()
		{
			try
			{
				SendRequest<ResultObject, RequestObject, ErrorResultObject>(null);
			}
			catch (ArgumentException e)
            {
				if (e.Message != "Request Model cannot be null\r\nParameter name: data" || e.ParamName != "data")
				{
					Assert.Fail();
				}
			}
			catch (Exception)
			{
				Assert.Fail();
			}
		}

		[TestMethod]
		public void TestSendRequest()
		{
			RequestGetObject rgo = new RequestGetObject();

			MockHandler.AddFakeResponse(
				new RestUrlBuilder(Url, rgo),
				HttpStatusCode.OK,
				"{\"Field1\":\"expectedValue\"}"
			);

			ResultObject result = SendRequest<ResultObject, RequestGetObject, ErrorResultObject>(rgo);

			Assert.AreEqual<string>("expectedValue", result.Field1);
		}

		[TestMethod]
		[ExpectedException(typeof(RestErrorResponseException))]
		public void TestSendRequestGetInvalidResponse()
		{
			RequestGetObject rgo = new RequestGetObject();

			MockHandler.AddFakeResponse(
				new RestUrlBuilder(Url, rgo),
				HttpStatusCode.OK,
				"{{}{{}}\"]}"
			);

			ResultObject result = SendRequest<ResultObject, RequestGetObject, ErrorResultObject>(rgo);
		}

		[TestMethod]
		public void TestSendRequestGetErrorResponseObject()
		{
			RequestPostObject rpo = new RequestPostObject();

			MockHandler.AddFakeResponse(
				new RestUrlBuilder(Url, rpo),
				HttpStatusCode.Unauthorized,
				"{\"ErrorMessage\":\"ErrorMessageValue\"}"
			);

			try
			{
				ResultObject result = SendRequest<ResultObject, RequestPostObject, ErrorResultObject>(rpo);
				Assert.Fail("Error Response Exception didn't get thrown.");
			}
			catch (RestErrorResponseException e)
			{
				Assert.IsTrue(e.Error is ErrorResultObject);

				ErrorResultObject errorObject = (ErrorResultObject)e.Error;

				Assert.AreEqual<string>("ErrorMessageValue", errorObject.ErrorMessage);
			}
		}

		public RestClientBaseMockTest()
			: base(DataTypes.JSON)
		{
			MockHandler = new MockableResponseHandler();
		}

		public RestClientBaseMockTest(DataTypes dt)
			: base(dt)
		{
			MockHandler = new MockableResponseHandler();
		}

		public RestClientBaseMockTest(IRestDataSerializer serializer)
			: base(serializer)
		{
			MockHandler = new MockableResponseHandler();
		}

		~RestClientBaseMockTest()
		{
			if (MockHandler != null)
				MockHandler.Dispose();
			Dispose(false);
		}

		private MockableResponseHandler MockHandler { get; set; }

		protected override string Url { get { return "http://localhost/"; } }

		protected override HttpClient SetupConnection()
		{
			return new HttpClient(MockHandler);
		}

		[DataContract]
		private class RequestGetObject : RestGetRequest { }

		[DataContract]
		private class RequestPostObject : RestPostRequest { }

		[DataContract]
		private class RequestObject : RestRequest
        {
            internal override HttpMethod GetHttpMethodType()
            {
                throw new NotImplementedException();
            }
        }

		[DataContract]
		private class ResultObject : RestResult
		{
			[DataMember(Name = "Field1")]
			public string Field1 { get; set; }
		}

		[DataContract]
		private class ErrorResultObject : RestErrorResult
		{
			[DataMember(Name = "ErrorMessage")]
			public string ErrorMessage { get; set; }
		}

		private class MockableResponseHandler : DelegatingHandler
		{
			private readonly Dictionary<Uri, HttpResponseMessage> MockedResponses = new Dictionary<Uri, HttpResponseMessage>();

			public void AddFakeResponse(RestUrlBuilder uri, HttpStatusCode code, string data)
			{
				MockedResponses.Add(
					new Uri(uri.ToString()),
					new HttpResponseMessage(code)
					{
						Content = new ByteArrayContent(GetBytes(data))
					}
				);
			}

			protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
			{
				if (MockedResponses.ContainsKey(request.RequestUri))
				{
					return Task.FromResult(MockedResponses[request.RequestUri]);
				}
				else
				{
					return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)
					{
						RequestMessage = request
					});
				}
			}

			protected byte[] GetBytes(string str)
			{
				return System.Text.Encoding.UTF8.GetBytes(str);
			}
		}
	}

	[ExcludeFromCodeCoverage]
	internal class CustomSerializer : IRestDataSerializer
	{
		public virtual string ToDataType<RestRequestType>(RestRequestType model)
			where RestRequestType : RestRequest, new()
		{
			return string.Empty;
		}

		public virtual RestResultType FromDataType<RestResultType>(string data)
			where RestResultType : RestResult, new()
		{
			return default(RestResultType);
		}

		public void Dispose()
		{
		}
	}

	[ExcludeFromCodeCoverage]
	[TestClass]
	public class RestClientBaseTests
	{
		[TestMethod]
		public void ConstructorTestXml()
		{
			using (RestClientBaseMockTest mock = new RestClientBaseMockTest(RestClientBase.DataTypes.XML)) { }
		}

		[TestMethod]
		public void ConstructorTestCustomSerializer()
		{
			using (RestClientBaseMockTest mock = new RestClientBaseMockTest(new CustomSerializer())) { }
		}

		[TestMethod]
		[ExpectedException(typeof(RestClientDotNetException))]
		public void ConstructorTestNullCustomSerializer()
		{
			using (RestClientBaseMockTest mock = new RestClientBaseMockTest(null)) { }
		}

		[TestMethod]
		public void SafeDoubleDispose()
		{
			using (RestClientBaseMockTest mock = new RestClientBaseMockTest(new CustomSerializer()))
			{
				mock.Dispose();
			}
		}
	}
}
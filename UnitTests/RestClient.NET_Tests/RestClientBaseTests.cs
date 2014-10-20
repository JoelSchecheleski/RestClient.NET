using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkaCahToa.Rest.Exceptions;
using SkaCahToa.Rest.Models;
using SkaCahToa.Rest.Web;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SkaCahToa.Rest.Tests
{
    [TestClass]
    public class RestClientBaseMockTest : RestClientBase
    {
        public RestClientBaseMockTest() : base(DataTypes.JSON)
        {
            MockHandler = new MockableResponseHandler();
        }

        private MockableResponseHandler MockHandler { get; set; }

        protected override string Url { get { return "http://localhost/"; } }

        protected override HttpClient SetupCreds(HttpClient hc)
        {
            return new HttpClient(MockHandler);
        }

        [TestMethod]
        public void TestSendRequestThrowExceptionIfNoModelMethodIsSelected()
        {
            try
            {
                SendRequest<ResultObject, RequestObject, ErrorResultObject>(null);
            }
            catch (AggregateException e)
            {
                if (!(e.InnerException is RestClientDotNetException))
                {
                    Assert.Fail();
                }
                else if (e.InnerException.Message != "Http Method Type Not Supported")
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void TestSendRequest()
        {
            RequestGetObject rgo = new RequestGetObject();

            MockHandler.AddFakeResponse(
                rgo.GetModelURL(Url),
                HttpStatusCode.OK,
                "{\"Field1\":\"expectedValue\"}"
            );

            ResultObject result = SendRequest<ResultObject, RequestGetObject, ErrorResultObject>(rgo);

            Assert.AreEqual<string>("expectedValue", result.Field1);
        }

        private class RequestGetObject : RestGetRequest { }

        private class RequestObject : RestRequest { }

        private class ResultObject : RestResult
        {
            public string Field1 { get; set; }
        }

        private class ErrorResultObject : RestErrorResult { }

        private class MockableResponseHandler : DelegatingHandler
        {
            private readonly Dictionary<Uri, HttpResponseMessage> MockedResponses = new Dictionary<Uri, HttpResponseMessage>();

            public void AddFakeResponse(RestUrl uri, HttpStatusCode code, string data)
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
}
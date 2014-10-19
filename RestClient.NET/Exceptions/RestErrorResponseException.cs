using SkaCahToa.Rest.Models;

namespace SkaCahToa.Rest.Exceptions
{
    public class RestErrorResponseException : RestClientDotNetException
    {
        public RestErrorResult Error { get; private set; }

        public RestErrorResponseException(RestErrorResult error, string message) : base(message)
        {
            Error = error;
        }
    }
}
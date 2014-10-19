using System;

namespace SkaCahToa.Rest.Exceptions
{
    public class RestClientDotNetException : Exception
    {
        public RestClientDotNetException(string message) : base(message)
        {
        }
    }
}
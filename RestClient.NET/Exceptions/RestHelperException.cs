using System;

namespace SkaCahToa.Rest.Exceptions
{
    public class RestHelperException : Exception
    {
        public RestHelperException(string message) : base(message)
        {
        }
    }
}
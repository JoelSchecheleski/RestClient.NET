using SkaCahToa.Rest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkaCahToa.Rest.Exceptions
{
    public class RestErrorResponseException<ErrorType> : RestHelperException
        where ErrorType : RestErrorResult
    {
        public ErrorType Error { get; private set; }

        public RestErrorResponseException(ErrorType error, string message) : base(message)
        {
            Error = error;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkaCahToa.Rest.Exceptions
{
    public class RestHelperException : Exception
    {
        public RestHelperException(string message) : base(message)
        {

        }
    }
}

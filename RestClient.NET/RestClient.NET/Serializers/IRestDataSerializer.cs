using SkaCahToa.Rest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkaCahToa.Rest.Serializers
{
    public interface IRestDataSerializer
    {
        string ToDataType<RestRequestType>(RestRequestType model)
            where RestRequestType : RestRequest;
        RestResultType FromDataType<RestResultType>(string data)
            where RestResultType : RestResult;
    }
}

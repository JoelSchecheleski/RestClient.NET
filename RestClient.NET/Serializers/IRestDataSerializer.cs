using SkaCahToa.Rest.Models;
using System;

namespace SkaCahToa.Rest.Serializers
{
    public interface IRestDataSerializer : IDisposable
    {
        string ToDataType<RestRequestType>(RestRequestType model)
            where RestRequestType : RestRequest;

        RestResultType FromDataType<RestResultType>(string data)
            where RestResultType : RestResult;
    }
}
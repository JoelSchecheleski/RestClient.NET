using SkaCahToa.Rest.Models;

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
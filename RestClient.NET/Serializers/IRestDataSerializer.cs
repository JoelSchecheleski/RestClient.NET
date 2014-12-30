using SkaCahToa.Rest.Models;
using System;

namespace SkaCahToa.Rest.Serializers
{
	/// <summary>
	/// Interface that'll Serialize and Deseralize RestModels.
	/// </summary>
	public interface IRestDataSerializer : IDisposable
	{
		/// <summary>
		/// Converts a RestRequestModel into a formatted string.
		/// </summary>
		/// <typeparam name="RestRequestType"></typeparam>
		/// <param name="model"></param>
		/// <returns></returns>
		string ToDataType<RestRequestType>(RestRequestType model)
			where RestRequestType : RestRequest, new();

		/// <summary>
		/// Converts a formatted string into a RestResultModel
		/// </summary>
		/// <typeparam name="RestResultType"></typeparam>
		/// <param name="data"></param>
		/// <returns></returns>
		RestResultType FromDataType<RestResultType>(string data)
			where RestResultType : RestResult, new();
	}
}
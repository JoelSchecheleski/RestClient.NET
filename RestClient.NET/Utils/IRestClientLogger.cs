using System;

namespace SkaCahToa.Rest.Utils
{
    /// <summary>
    /// Error Logging Interface used by RestClient.NET. Implement this on your Logging System to have RCDN populate logging messages.
    /// </summary>
    public interface IRestClientLogger
    {
        void LogException(Exception e);
    }
}

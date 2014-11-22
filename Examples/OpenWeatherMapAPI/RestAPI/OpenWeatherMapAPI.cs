using Newtonsoft.Json;
using SkaCahToa.Rest;
using SkaCahToa.Rest.Models;
using SkaCahToa.Rest.Models.Attributes;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenWeatherMap.RestAPI
{
    public class OpenWeatherMapAPI : RestClientBase
    {
        public OpenWeatherMapAPI() : base(DataTypes.JSON)
        {
        }

        protected override string Url
        {
            get
            {
                return "http://api.openweathermap.org/data/2.5/";
            }
        }

        protected override HttpClient SetupConnection()
        {
            return new HttpClient();
        }

        public WeatherResult GetWeather(string query)
        {
            return SendRequest<WeatherResult, WeatherGetRequest, WeatherErrorResult>(new WeatherGetRequest()
            {
                Query = query
            });
        }

        public Task<WeatherResult> GetWeatherAsync(string query)
        {
            return SendRequestAsync<WeatherResult, WeatherGetRequest, WeatherErrorResult>(new WeatherGetRequest()
            {
                Query = query
            });
        }

        [SegmentDef(1, UrlDefinitionDataTypes.Static, "weather")]
        [ParameterDef("q", UrlDefinitionDataTypes.Data, "Query")]
        protected class WeatherGetRequest : RestGetRequest
        {
            public string Query { get; set; }
        }

        public class WeatherResult : RestResult
        {
			[XmlElement("name")]
			[JsonProperty("name")]
            public string Name { get; set; }

			[XmlElement("dt")]
			[JsonProperty("dt")]
            public int TimeStamp { get; set; }

			[XmlElement("id")]
			[JsonProperty("id")]
            public int ID { get; set; }

			[XmlElement("cod")]
			[JsonProperty("cod")]
            public int Cod { get; set; }
        }

        protected class WeatherErrorResult : RestErrorResult
        {
        }
    }
}
### RestClient.NET

A .NET Library aimed to simplify writing REST API Clients.

## Example

```csharp
	public class OpenWeatherMapAPI : RestClientBase
	{
		public OpenWeatherMapAPI()
			: base(DataTypes.JSON)
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

		[DataContract]
		[SegmentDef(1, UrlDefinitionDataTypes.Static, "weather")]
		[ParameterDef("q", UrlDefinitionDataTypes.Data, "Query")]
		protected class WeatherGetRequest : RestGetRequest
		{
			public string Query { get; set; }
		}

		[DataContract]
		public class WeatherResult : RestResult
		{
			[DataMember(Name = "name")]
			public string Name { get; set; }

			[DataMember(Name = "dt")]
			public int TimeStamp { get; set; }

			[DataMember(Name = "id")]
			public int ID { get; set; }

			[DataMember(Name = "cod")]
			public int Cod { get; set; }
		}

		[DataContract]
		protected class WeatherErrorResult : RestErrorResult
		{
		}
	}

using (OpenWeatherMapAPI api = new OpenWeatherMapAPI())
{
	//Make a Sync call.
	OpenWeatherMapAPI.WeatherResult result = api.GetWeather("London,Ca");
	//Use result data
	Console.WriteLine(result.Name + " " + result.TimeStamp);
	//Make an Async call.
	Task<OpenWeatherMapAPI.WeatherResult> task = api.GetWeatherAsync("Fenton,Mi");
	//do other stuff
	Console.WriteLine("OtherStuff");
	//Use result data
	task.Wait();
	Console.WriteLine(task.Result.Name + " " + task.Result.TimeStamp);
}
```

## License

RestClient.NET is released under the MIT License: https://tldrlegal.com/license/mit-license
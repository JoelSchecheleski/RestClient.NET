using OpenWeatherMap.RestAPI;
using System;
using System.Threading.Tasks;

namespace OpenWeatherMap
{
    internal class Program
    {
		private static void Main(string[] args)
		{
			//Get our API Class.
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
		}
    }
}
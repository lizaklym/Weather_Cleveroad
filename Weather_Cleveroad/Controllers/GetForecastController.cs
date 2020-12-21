using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Weather_Cleveroad.Models;
using Weather_Cleveroad.Models.WeatherForecast;

namespace Weather_Cleveroad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetForecastController : ControllerBase
    {
        private static object locker = new object();
        public static async Task<string> GetWeatherForFiveDays(string city)
        {
            if (city == null)
                return StatusCodes.Status400BadRequest.ToString();

            var codeCountry = await Task.Run(() => GetCodeCountry(city));
            var http = new HttpClient();
            var response = await http.GetAsync($"http://api.openweathermap.org/data/2.5/forecast?q={city},{codeCountry}&appid=b3c828810d3a5a76bc521cf9479b61a4&units=metric");
            lock (locker)
            {
                if (response.IsSuccessStatusCode == true)
                {
                    var result = response.Content.ReadAsStringAsync();

                    var serializer = new DataContractJsonSerializer(typeof(GetForecastCity));
                    var memory_stream = new MemoryStream(Encoding.UTF8.GetBytes(result.Result.ToCharArray()));
                    var data = (GetForecastCity)serializer.ReadObject(memory_stream);
                    return data.ToString();
                }
                return StatusCodes.Status404NotFound.ToString();
            }
        }

        /// <summary>
        /// Get forecast for specific city
        /// </summary>
        /// <param name="city"></param>
        /// <returns>Forecast for specific city</returns>
        /// <response code="400">If the city is null</response>
        /// <response code="404">Not found city</response>
        /// <response code="500">Server Error!</response>   
        /// <response code="200">Wow It is Ok!</response> 
        [HttpGet]
        [Route("api/[controller]")]
        public async Task<IActionResult> Get(string city)
        {
            if (city != null)
            {
                var weather = await Task.Run(() => GetWeatherForFiveDays(city));
                //var weather = await GetWeatherForFiveDays(city);
                if (weather.Contains("404"))
                {
                    return NotFound(weather.ToString());
                }
                return Ok(weather.ToString());
            }
            return BadRequest();
        }

        private static string GetCodeCountry(string city)
        {
            lock (locker)
            {
                var http = new HttpClient();
                var response = http.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q={city}&appid=b3c828810d3a5a76bc521cf9479b61a4&units=metric");
                var result = response.Result.Content.ReadAsStringAsync();

                var serializer = new DataContractJsonSerializer(typeof(GetCurrentCity));
                var memory_stream = new MemoryStream(Encoding.UTF8.GetBytes(result.Result.ToCharArray()));
                var data = (GetCurrentCity)serializer.ReadObject(memory_stream);

                return data.sys.country;
            }
        }
    }
}
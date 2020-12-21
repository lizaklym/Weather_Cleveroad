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

namespace Task_WeatherView.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetCurrentWeatherController : ControllerBase
    {
        private static object locker = new object();
        public static string GetWeather(string city)
        {
            lock (locker)
            {
                if (city == null)
                    return StatusCodes.Status400BadRequest.ToString();

                var http = new HttpClient();
                var response = http.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q={city}&appid=b3c828810d3a5a76bc521cf9479b61a4&units=metric");
                if (response.Result.IsSuccessStatusCode == true)
                {
                    var result = response.Result.Content.ReadAsStringAsync();

                    var serializer = new DataContractJsonSerializer(typeof(GetCurrentCity));
                    var memory_stream = new MemoryStream(Encoding.UTF8.GetBytes(result.Result.ToCharArray()));
                    var data = (GetCurrentCity)serializer.ReadObject(memory_stream);
                    return data.ToString();
                }
            }
            return StatusCodes.Status404NotFound.ToString();
        }
        /// <summary>
        /// Get weather for spicific city
        /// </summary>
        /// <param name="city"></param>
        /// <returns>Get weather</returns>
        /// <response code="400">If the city is null</response>
        /// <response code="404">Not found city</response>
        /// <response code="500">Server Error!</response>   
        /// <response code="200">Wow It is Ok!</response>   
        [HttpGet]
        public IActionResult Get(string city)
        {
            if (city != null)
            {
                Task<string> task = Task.Run(() => GetWeather(city));

                //var weather = GetWeather(city);
                if (task.Result.Contains("404"))
                {
                    return NotFound(task.Result.ToString());
                }
                return Ok(task.Result.ToString());
            }
            return BadRequest();
        }
    }
}
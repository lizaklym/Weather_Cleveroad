using System.Collections.Generic;
using System.Text;

namespace Weather_Cleveroad.Models
{
    namespace WeatherForecast
    {
        public class Main
        {
            public double temp { get; set; }
            public double temp_min { get; set; }
            public double temp_max { get; set; }
            public double pressure { get; set; }
            public double sea_level { get; set; }
            public double grnd_level { get; set; }
            public int humidity { get; set; }
            public double temp_kf { get; set; }
        }

        public class Weather
        {
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }

        public class Clouds
        {
            public int all { get; set; }
        }

        public class Wind
        {
            public double speed { get; set; }
            public double deg { get; set; }
        }

        public class Sys
        {
            public string pod { get; set; }
        }

        public class Rain
        {
            public double __invalid_name__3h { get; set; }
        }

        public class Snow
        {
            public double __invalid_name__3h { get; set; }
        }

        public class List
        {
            public int dt { get; set; }
            public Main main { get; set; }
            public List<Weather> weather { get; set; }
            public Clouds clouds { get; set; }
            public Wind wind { get; set; }
            public Sys sys { get; set; }
            public string dt_txt { get; set; }
            public Rain rain { get; set; }
            public Snow snow { get; set; }
        }

        public class Coord
        {
            public double lat { get; set; }
            public double lon { get; set; }
        }

        public class City
        {
            public int id { get; set; }
            public string name { get; set; }
            public Coord coord { get; set; }
            public string country { get; set; }
        }

        public class GetForecastCity
        {
            public string cod { get; set; }
            public double message { get; set; }
            public int cnt { get; set; }
            public List<List> list { get; set; }
            public City city { get; set; }
            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 4; i < list.Count; i += 8)
                {
                    builder.Append("Date: " + list[i].dt_txt + "\n");
                    builder.Append("Min temp: " + list[i].main.temp_min + " °C\n");
                    builder.Append("Max temp: " + list[i].main.temp_max + " °C\n");
                    builder.Append("Wind speed: " + list[i].wind.speed + " km/h\n");
                    builder.Append("Clouds: " + list[i].weather[0].description + "\n");
                    builder.Append("-----------------------------------------\n");
                }
                string overallInfo = builder.ToString();
                return overallInfo;
            }
        }
    }
}

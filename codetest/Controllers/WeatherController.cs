using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace codetest
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        string strWeather = "[{\"date\":\"2021-04-30T00:08:43.4486876+12:00\",\"temperatureC\":-3,\"temperatureF\":27,\"summary\":\"Scorching\"},{\"date\":\"2021-05-01T00:08:43.451029+12:00\",\"temperatureC\":48,\"temperatureF\":118,\"summary\":\"Balmy\"},{ \"date\":\"2021-05-02T00:08:43.4510322+12:00\",\"temperatureC\":4,\"temperatureF\":39,\"summary\":\"Mild\"},{ \"date\":\"2021-05-03T00:08:43.4510327+12:00\",\"temperatureC\":17,\"temperatureF\":62,\"summary\":\"Freezing\"},{ \"date\":\"2021-05-04T00:08:43.451033+12:00\",\"temperatureC\":22,\"temperatureF\":71,\"summary\":\"Scorching\"}]";

        [HttpGet]
        [Route("GetWeatherForecast")]
        public string GetWeatherForecast()
        {

            string savedTime = string.Empty;
            if (HttpContext.Session.GetString("Key") != null)
            {
                savedTime = HttpContext.Session.GetString("Key");
            }
            else
            {
                HttpContext.Session.SetString("Key", DateTime.Now.ToString());
                savedTime = HttpContext.Session.GetString("Key");
            }

            var waitingTime = DateTime.Parse(savedTime);
            waitingTime = waitingTime.AddMinutes(10);

            if (DateTime.Now <= waitingTime)
            {
                return strWeather;
            }
            {
                return "10mins plus, need to change value";
            }
        }


        [HttpPost]
        [Route("AddWeatherForecast")]
        public string AddWeatherForecast(string obj)
        {
            var list = JsonConvert.DeserializeObject<List<Weather>>(obj);
            Weather wth = new Weather
            {
                date = DateTime.Now.ToString(),
                temperatureC = "4",
                temperatureF = "6",
                summary = "raining"
            };
            list.Add(wth);

            var descListOb = list.OrderBy(x => x.date);
            return JsonConvert.SerializeObject(descListOb);
        }

    }
}

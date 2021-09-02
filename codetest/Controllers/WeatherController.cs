using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private IMemoryCache memoryCache;
        private readonly string weatherForecastKey = "weatherForecastKey";
        List<Weather> lsweather = null;
        private int expiredDuration = 10;
        public WeatherController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("GetWeatherForecast")]
        public IEnumerable<Weather> GetWeatherForecast()
        {   
            string strWeatherForecast;
            if (isDataExist())
            {
                return lsweather;
            }
            else
            {
                strWeatherForecast = "[{\"date\":\"2021-04-30T00:08:43.4486876+12:00\",\"temperatureC\":-3,\"temperatureF\":27,\"summary\":\"Scorching\"},{\"date\":\"2021-05-01T00:08:43.451029+12:00\",\"temperatureC\":48,\"temperatureF\":118,\"summary\":\"Balmy\"},{ \"date\":\"2021-05-02T00:08:43.4510322+12:00\",\"temperatureC\":4,\"temperatureF\":39,\"summary\":\"Mild\"},{ \"date\":\"2021-05-03T00:08:43.4510327+12:00\",\"temperatureC\":17,\"temperatureF\":62,\"summary\":\"Freezing\"},{ \"date\":\"2021-05-04T00:08:43.451033+12:00\",\"temperatureC\":22,\"temperatureF\":71,\"summary\":\"Scorching\"}]";
                lsweather = JsonConvert.DeserializeObject<List<Weather>>(strWeatherForecast);
                SetToMemoryCache();
            }
            return lsweather;
        }


        [HttpPost]
        [Route("AddWeatherForecast")]
        public IEnumerable<Weather> AddWeatherForecast([FromBody] Weather weather)
        {
            if (isDataExist())
            {
                return lsweather;
            }
            else
            {
                var rdn = new Random();
                if (lsweather != null)
                {
                    lsweather.Add(weather);
                }
                else
                {
                    lsweather = new List<Weather>();
                    lsweather.Add(weather);
                }

                lsweather = lsweather.OrderBy(x => x.date).ToList();
                SetToMemoryCache();
                return lsweather;
            }
        }

        private bool isDataExist()
        {
            return memoryCache.TryGetValue(weatherForecastKey, out lsweather);
        }

        private void SetToMemoryCache()
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(expiredDuration))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(expiredDuration));
            memoryCache.Set(weatherForecastKey, lsweather, cacheOptions);
        }
    }
}

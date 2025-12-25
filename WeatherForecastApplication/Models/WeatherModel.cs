using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherForecastApplication.Models
{
    public class WeatherModel
    {
        public string City { get; set; }
        public string Description { get; set; }
        public double Temperature { get; set; }
        public double MinTemperature { get; set; }
        public double MaxTemperature { get; set; }
        public double Humidity { get; set; }
    }
}
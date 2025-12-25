using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WeatherForecastApplication.Models;

namespace WeatherForecastApplication.Controllers
{
    public class WeatherForecastController : Controller
    {
        WeatherForecastEntities2 db = new WeatherForecastEntities2();
        // GET: WeatherForecast
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            user = new User();
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                // Query to check if email and password match in the database
                var user = db.Users
                    .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    // If user exists, redirect to Weather action
                    return RedirectToAction("Weather");
                }
                else
                {
                    // If credentials don't match, show toast error
                    TempData["ErrorMessage"] = "Invalid email or password.";
                }
            }

            //return RedirectToAction("Weather");
            return View(model);
        }
        

        public ActionResult Weather()
        {
            return View();
        }

        [HttpPost]
        public String WeatherDetail(string City)
        {

            string appId = "8113fcc5a7494b0518bd91ef3acc074f";


            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&cnt=1&APPID={1}", City, appId);

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);

                RootObject weatherInfo = (new JavaScriptSerializer()).Deserialize<RootObject>(json);

                ResultViewModel rslt = new ResultViewModel();

                rslt.Country = weatherInfo.sys.country;
                rslt.City = weatherInfo.name;
                rslt.Lat = Convert.ToString(weatherInfo.coord.lat);
                rslt.Lon = Convert.ToString(weatherInfo.coord.lon);
                rslt.Description = weatherInfo.weather[0].description;
                rslt.Humidity = Convert.ToString(weatherInfo.main.humidity);
                rslt.Temp = Convert.ToString(weatherInfo.main.temp);
                rslt.TempFeelsLike = Convert.ToString(weatherInfo.main.feels_like);
                rslt.TempMax = Convert.ToString(weatherInfo.main.temp_max);
                rslt.TempMin = Convert.ToString(weatherInfo.main.temp_min);
                rslt.WeatherIcon = weatherInfo.weather[0].icon;

                var jsonstring = new JavaScriptSerializer().Serialize(rslt);

                return jsonstring;
            }
        }

    }
}
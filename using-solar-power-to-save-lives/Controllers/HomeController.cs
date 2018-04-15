using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using using_solar_power_to_save_lives.Models;

namespace using_solar_power_to_save_lives.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View("About");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View("Contact");
        }

        public IActionResult ParabolicSolarCookerHowTo()
        {
            ViewData["Message"] = "Your Parabola Calculator page.";

            return View("ParabolicSolarCookerHowTo");
        }

        public IActionResult ParabolicSolarCookerCalculator()
        {
            ViewData["Message"] = "Your Parabola Calculator page.";

            return View("ParabolicSolarCookerCalculator");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

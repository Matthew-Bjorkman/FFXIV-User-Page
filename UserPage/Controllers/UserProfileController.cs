using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UserPage.Models;

namespace UserPage.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public UserProfileController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            UserProfileViewModel vm = new UserProfileViewModel();

            return View(vm);
        }

        public IActionResult Details(int id)
        {
            UserProfileViewModel vm = new UserProfileViewModel()
            {
                Id = id
            };

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

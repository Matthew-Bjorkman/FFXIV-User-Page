using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UserPage.Models;
using UserPage.Utilities;

namespace UserPage.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _cache;
        private readonly ILodestoneAPIClient _client;

        public UserProfileController(ILogger<HomeController> logger, ILodestoneAPIClient client, IMemoryCache cache)
        {
            _logger = logger;
            _client = client;
            _cache = cache;
        }

        public IActionResult Index()
        {
            UserSearchDto vm = new UserSearchDto();

            return View(vm);
        }

        public IActionResult Details(int id)
        {
            UserSearchDto vm = new UserSearchDto()
            {
                
            };

            return View(vm);
        }

        public IActionResult List(UserSearchDto model)
        {
            if (!ModelState.IsValid)
                return View("Index");

            string cacheKey = $"UserSeach-{model.Name}-{model.Server}";

            UserSearchViewModel vm = null;
            if (!_cache.TryGetValue(cacheKey, out vm))
            {
                IEnumerable<LodestoneCharacterModel> lodestoneCharacters = null;
                try
                {
                    lodestoneCharacters = _client.CharacterSearch(model, 50);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    return RedirectToAction("Error");
                }

                vm = new UserSearchViewModel
                {
                    UserList = lodestoneCharacters.Select(x => new UserSearchListItem
                    {
                        Name = x.Name, 
                        Server = x.Server,
                        AvatarUrl = x.Avatar
                    }).ToList()
                };

                _cache.Set(cacheKey, vm, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1)));
            }

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

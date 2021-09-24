using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UserPage.Models;

namespace UserPage.Utilities
{
    public class LodestoneAPIClient : ILodestoneAPIClient
    {
        private HttpClient _client;
        public LodestoneAPIClient(HttpClient client)
        {
            _client = client;
        }

        private object Get(string requestUrl)
        {
            HttpResponseMessage req = _client.GetAsync(requestUrl).Result;
            dynamic item = JsonConvert.DeserializeObject(req.Content.ReadAsStringAsync().Result);

            return item;
        }

        public IEnumerable<LodestoneCharacterModel> CharacterSearch(UserSearchDto request, int maxResults = 10)
        {
            List<LodestoneCharacterModel> lodestoneCharacters = new List<LodestoneCharacterModel>();

            string requestUri = $"/character/search?name={request.Name.Replace(" ", "+")}&server={request.Server}";
            dynamic result = Get(requestUri);

            IEnumerable<dynamic> resultList = result.Results;

            foreach (dynamic characterResult in resultList)
            {
                if (maxResults-- <= 0)
                    break;

                LodestoneCharacterModel lodestoneCharacter = new LodestoneCharacterModel
                {
                    Name = characterResult.Name,
                    Server = characterResult.Server,
                    Avatar = characterResult.Avatar
                };

                lodestoneCharacters.Add(lodestoneCharacter);
            }

            return lodestoneCharacters;
        }
    }

    public interface ILodestoneAPIClient
    {
        IEnumerable<LodestoneCharacterModel> CharacterSearch(UserSearchDto request, int maxResults = 10);
    }
}

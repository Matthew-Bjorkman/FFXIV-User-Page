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
            dynamic item = null;

            try
            {
                HttpResponseMessage req = _client.GetAsync(requestUrl).Result;
                item = JsonConvert.DeserializeObject(req.Content.ReadAsStringAsync().Result);
            }
            catch (Exception e)
            {
                //TODO
            }
            
            return item;
        }

        public IEnumerable<LodestoneCharacterSearchModel> CharacterSearch(CharacterSearchDto request, int maxResults = 10)
        {
            List<LodestoneCharacterSearchModel> lodestoneCharacters = new List<LodestoneCharacterSearchModel>();

            string requestUri = $"/character/search?name={request.Name.Replace(" ", "+")}&server={request.Server}";
            dynamic result = Get(requestUri);

            IEnumerable<dynamic> resultList = result.Results;

            foreach (dynamic characterResult in resultList)
            {
                if (maxResults-- <= 0)
                    break;

                LodestoneCharacterSearchModel lodestoneCharacter = new LodestoneCharacterSearchModel
                {
                    Name = characterResult.Name,
                    Server = characterResult.Server,
                    Avatar = characterResult.Avatar,
                    ID = characterResult.ID,
                    FeastMatches = characterResult.FeastMatches,
                    Lang = characterResult.Lang,
                    Rank = characterResult.Rank,
                    RankIcon = characterResult.RankIcon
                };

                lodestoneCharacters.Add(lodestoneCharacter);
            }

            return lodestoneCharacters;
        }

        public LodestoneCharacterModel Character(CharacterDto request)
        {
            string requestUri = $"/character/{request.Id}?data=";
            if (request.IncludeAchievements)
                requestUri += "AC,";
            if (request.IncludeFriendsList)
                requestUri += "FR,";
            if (request.IncludeFreeCompany)
                requestUri += "FC,";
            if (request.IncludeFreeCompanyMembers)
                requestUri += "FCM,";
            if (request.IncludeMountsAndMinions)
                requestUri += "MIMO,";
            if (request.IncludePVPTeam)
                requestUri += "PVP";

            dynamic result = Get(requestUri);

            LodestoneCharacterModel lodestoneCharacter = new LodestoneCharacterModel
            {
                Avatar = result.Character.Avatar,
                Portrait = result.Character.Portrait,
                ID = result.Character.ID,
                Name = result.Character.Name,
                Server = result.Character.Server
            };

            return lodestoneCharacter;
        }
    }

    public interface ILodestoneAPIClient
    {
        IEnumerable<LodestoneCharacterSearchModel> CharacterSearch(CharacterSearchDto request, int maxResults = 10);
        LodestoneCharacterModel Character(CharacterDto request);
    }
}

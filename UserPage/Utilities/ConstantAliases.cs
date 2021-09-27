using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserPage.Utilities
{
    public class ApplicationConfigurationFields
    {
        public const string LodestoneApiUrl = "ApplicationConfiguration:LodestoneApiUri";
        public const string DiscordClientId = "ApplicationConfiguration:DiscordClientId";
        public const string DiscordClientSecret = "ApplicationConfiguration:DiscordClientSecret";
    }

    public class SessionFields
    {
        public const string LastUserSearch = "LastUserSearch";
    }
}

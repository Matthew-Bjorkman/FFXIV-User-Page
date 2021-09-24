using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserPage
{
    public class ApplicationConfiguration
    {
        public string LodestoneApiUri { get; set; }
    }

    public class ApplicationConfigurationFields
    {
        public const string LodestoneApiUrl = "ApplicationConfiguration:LodestoneApiUri";
    }
}

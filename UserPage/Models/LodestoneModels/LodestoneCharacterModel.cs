using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserPage.Models
{
    // endpoint: /character/[lodestone_id]
    public class LodestoneCharacterModel
    {
        public string Avatar { get; set; }
        public string Portrait { get; set; }
        public int ID { get; set; }
        public string Lang { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
    }
}

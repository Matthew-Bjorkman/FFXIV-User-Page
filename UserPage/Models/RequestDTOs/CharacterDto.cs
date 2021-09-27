using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserPage.Models
{
    // endpoint: /character/[lodestone_id]
    public class CharacterDto
    {
        [Required]
        public int Id { get; set; }
        public bool IncludeAchievements { get; set; }
        public bool IncludeFriendsList { get; set; }
        public bool IncludeFreeCompany { get; set; }
        public bool IncludeFreeCompanyMembers { get; set; }
        public bool IncludeMountsAndMinions { get; set; }
        public bool IncludePVPTeam { get; set; }
    }
}

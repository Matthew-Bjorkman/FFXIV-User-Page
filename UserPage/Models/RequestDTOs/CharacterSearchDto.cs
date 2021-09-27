using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserPage.Models
{
    // endpoint: /character/search
    public class CharacterSearchDto
    {
        [StringLength(21, MinimumLength = 1)]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string Server { get; set; }
    }
}

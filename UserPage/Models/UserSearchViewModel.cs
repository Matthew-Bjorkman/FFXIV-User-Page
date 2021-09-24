using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserPage.Models
{
    public class UserSearchViewModel
    {
        public List<UserSearchListItem> UserList { get; set; }
    }

    public class UserSearchListItem
    {
        public string AvatarUrl { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
    }
}

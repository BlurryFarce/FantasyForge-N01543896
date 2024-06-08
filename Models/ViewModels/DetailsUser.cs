using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FantasyForge_N01543896.Models.ViewModels {
    public class DetailsUser {
        public UserDto SelectedUser { get; set; }
        public IEnumerable<UserMediaItem> UserPersonalList { get; set; }
    }
}
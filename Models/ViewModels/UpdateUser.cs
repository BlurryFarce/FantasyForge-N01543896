using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FantasyForge_N01543896.Models.ViewModels {
    public class UpdateUser {
        //This viewmodel is a class which stores information that we need to present to /User/Update/{}

        //the existing animal information

        public UserDto SelectedUser { get; set; }

        // all mediaitems to choose from when updating this user

        public IEnumerable<MediaItemDto> MediaItems { get; set; }
    }
}
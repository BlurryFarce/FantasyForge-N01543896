using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FantasyForge_N01543896.Models.ViewModels {
    public class DetailsMediaItem {
        //the species itself that we want to display
        public MediaItemDto SelectedMediaItem { get; set; }

        //all of the related animals to that particular species
        public IEnumerable<MediaItemDto> RelatedMediaItems { get; set; }
    }
}
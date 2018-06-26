using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cardid.Models
{
    public class Stats
    {
        public Dictionary<string, int> ActiveUsers { get; set; }
        public Dictionary<Deck, int> ActiveDecks { get; set; }
        public Dictionary<Tag, int> PopularTags { get; set; }
    }
}
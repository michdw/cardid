using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cardid.Models
{
    public class Stats
    {
        public Dictionary<string, int> ActiveUsers { get; set; }
        public Dictionary<string, int> ActiveDecks { get; set; }
        public Dictionary<string, int> PopularTags { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cardid.Models
{
    public class Search
    {
        public string SearchString { get; set; }
        public string UserID { get; set; }
        public List<Card> MatchingCards { get; set; }
        public List<Deck> MatchingDecks { get; set; }
        public List<Tag> MatchingTags { get; set; }
        public bool UserCards { get; set; }
        public bool UserDecks { get; set; }
        public bool PublicCards { get; set; }
        public bool PublicDecks { get; set; }

    }
}
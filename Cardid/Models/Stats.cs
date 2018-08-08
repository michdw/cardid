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

        public List<string> ActiveUsersOrder
        {
            get
            {
                List<string> names = new List<string>();
                foreach (string name in ActiveUsers.Keys)
                {
                    names.Add(name);
                }
                return names;
            }
        }

        public List<Deck> ActiveDecksOrder
        {
            get
            {
                List<Deck> names = new List<Deck>();
                foreach (Deck deck in ActiveDecks.Keys)
                {
                    names.Add(deck);
                }
                return names;
            }
        }

        public List<Tag> PopularTagsOrder
        {
            get
            {
                List<Tag> tags = new List<Tag>();
                foreach (Tag tag in PopularTags.Keys)
                {
                    tags.Add(tag);
                }
                return tags;
            }
        }
    }
}
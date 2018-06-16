using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Cardid.DAL;

namespace Cardid.Models
{
    public class Tag
    {
        public string TagID { get; set; }
        public string TagName { get; set; }
        public string UserID { get; set; }


        private string connectionString = ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString;

        public List<Deck> Decks()
        {
            DeckSqlDAL deckSql = new DeckSqlDAL(connectionString);
            return deckSql.GetDecksByTagID(TagID);
        }

        public HashSet<string> CurrentUserIDs()
        {
            HashSet<string> userIDs = new HashSet<string>();
            foreach (Deck deck in Decks())
            {
                userIDs.Add(deck.UserID);
            }
            return userIDs;
        }

    }
}
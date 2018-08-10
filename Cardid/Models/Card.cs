using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cardid.DAL;
using Cardid.Models;

namespace Cardid.Models
{
    public class Card
    {
        public string CardID { get; set; }
        public string UserID { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }

        public string CurrentDeckID { get; set; }
        public string CurrentSearchString { get; set; }

        public Card TrimValues()
        {
            CardID = CardID.Trim();
            UserID = UserID.Trim();
            Front = Front.Trim();
            Back = Back.Trim();
            return this;
        }

        private string connectionString = ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString;

        public List<Deck> Decks()
        {
            DeckSqlDAL deckSql = new DeckSqlDAL(connectionString);
            return deckSql.GetDecksByCardID(CardID);
        }

        public List<Deck> AvailableDecks()
        {
            DeckSqlDAL deckSql = new DeckSqlDAL(connectionString);
            return deckSql.DecksNotWithCard(CardID, UserID);
        }

    }
}
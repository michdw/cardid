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
        private string connectionString = ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString;

        public string CardID { get; set; }
        public string UserID { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }

        public List<Deck> Decks
        {
            get
            {
                DeckSqlDAL deckSql = new DeckSqlDAL(connectionString);
                return deckSql.GetDecksByCardID(CardID);
            }
        }

        public Card TrimValues()
        {
            CardID = CardID.Trim();
            UserID = UserID.Trim();
            Front = Front.Trim();
            Back = Back.Trim();
            return this;
        }

    }
}
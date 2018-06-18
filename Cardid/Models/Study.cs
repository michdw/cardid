using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Cardid.DAL;

namespace Cardid.Models
{
    public class Study
    {
        public string DeckID { get; set; }
        public string UserID { get; set; }
        public int TotalScore { get; set; }
        public int PossibleScore { get; set; }
        public DateTime TimeOf { get; set; }

        public string ToRedo { get; set; }

        public decimal Percentage()
        {
            return TotalScore / PossibleScore;
        }

        private string connectionString = ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString;

        public string DeckName()
        {
            DeckSqlDAL deckSql = new DeckSqlDAL(connectionString);
            Deck deck = deckSql.GetDeckByID(DeckID);
            return deck.DeckName;
        }
    }
}
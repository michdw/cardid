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

        public bool FrontFirst { get; set; }
        public bool WholeDeck { get; set; }

        public List<Card> Cards { get; set; }
        public string ToRedo { get; set; }


        private string connectionString = ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString;

        public int PercentScore
        {
            get
            {
                return (int)(Decimal.Divide(TotalScore, PossibleScore) * 100);
            }
        }

        public string DeckName
        {
            get
            {
                DeckSqlDAL deckSql = new DeckSqlDAL(connectionString);
                Deck deck = deckSql.GetDeckByID(DeckID);
                return deck.DeckName;
            }
        }

    }
}
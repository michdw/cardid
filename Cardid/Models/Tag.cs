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
        private string connectionString = ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString;

        public string TagID { get; set; }
        public string TagName { get; set; }
        public string UserID { get; set; }

        public List<Deck> DecksUsing
        {
            get
            {
                DeckSqlDAL deckSql = new DeckSqlDAL(connectionString);
                return deckSql.GetDecksByTagID(TagID);
            }
        }

    }
}
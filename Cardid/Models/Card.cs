﻿using System;
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


        readonly string connectionString = ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString;

        public Deck GetDeck()
        {
            DeckSqlDAL deckSql = new DeckSqlDAL(connectionString);
            return deckSql.GetDeckByCardID(CardID);
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
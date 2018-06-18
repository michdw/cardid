using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Cardid.DAL;

namespace Cardid.Models
{
    public class Deck
    {
        public string DeckID { get; set; }
        public string UserID { get; set; }
        public string DeckName { get; set; }
        public bool IsPublic { get; set; }

        public Tag NewTag { get; set; }

        public Deck TrimValues()
        {
            DeckID = DeckID.Trim();
            UserID = UserID.Trim();
            DeckName = DeckName.Trim();
            return this;
        }

        private string connectionString = ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString;

        public List<Card> Cards()
        {
            CardSqlDAL cardSql = new CardSqlDAL(connectionString);
            return cardSql.GetCardsByDeckID(DeckID);
        }

        public List<Tag> Tags()
        {
            TagSqlDAL tagSql = new TagSqlDAL(connectionString);
            return tagSql.GetTagsByDeckID(DeckID);
        }

        public User Creator()
        {
            UserSqlDAL userSql = new UserSqlDAL(connectionString);
            return userSql.GetUserByID(UserID);
        }

    }
}
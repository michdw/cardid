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
        private string connectionString = ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString;

        public string DeckID { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }

        public Tag NewTag { get; set; }

        public List<Card> Cards
        {
            get
            {
                CardSqlDAL cardSql = new CardSqlDAL(connectionString);
                return cardSql.GetCardsByDeckID(DeckID);
            }
        }

        public List<Tag> Tags
        {
            get
            {
                TagSqlDAL tagSql = new TagSqlDAL(connectionString);
                return tagSql.GetTagsByDeckID(DeckID);
            }
        }

        public User Creator
        {
            get
            {
                UserSqlDAL userSql = new UserSqlDAL(connectionString);
                return userSql.GetUserByID(UserID);
            }
        }

        public Deck TrimValues()
        {
            DeckID = DeckID.Trim();
            UserID = UserID.Trim();
            Name = Name.Trim();
            return this;
        }

    }
}
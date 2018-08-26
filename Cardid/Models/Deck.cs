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

        public string NewCardFront { get; set; }
        public string NewCardBack { get; set; }
        public Tag NewTag { get; set; }

        private string connectionString = ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString;

        public List<Card> Cards
        {
            get
            {
                CardSqlDAL cardSql = new CardSqlDAL(connectionString);
                return cardSql.GetCardsByDeckID(DeckID);
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

        public List<Tag> OtherTagsByName
        {
            get
            {
                TagSqlDAL tagSql = new TagSqlDAL(connectionString);
                return tagSql.GetOtherTagsByName(DeckID);
            }
        }

        public List<Tag> OtherTagsByPopularity
        {
            get
            {
                TagSqlDAL tagSql = new TagSqlDAL(connectionString);
                return tagSql.GetOtherTagsByPopularity(DeckID);
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

        public Deck TrimValues()
        {
            DeckID = DeckID.Trim();
            UserID = UserID.Trim();
            DeckName = DeckName.Trim();
            return this;
        }

    }
}
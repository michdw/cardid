using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Cardid.DAL;

namespace Cardid.Models
{
    public class Search
    {
        public string SearchString { get; set; }
        public string UserID { get; set; }
        public List<Card> MatchingCards { get; set; }
        public List<Deck> MatchingDecks { get; set; }
        public List<Tag> MatchingTags { get; set; }
        public bool UserCards { get; set; }
        public bool UserDecks { get; set; }
        public bool PublicCards { get; set; }
        public bool PublicDecks { get; set; }

        //private string connectionString = ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString;
        //public int DecksCount()
        //{
        //    UserSqlDAL userSql = new UserSqlDAL(connectionString);
        //    User user = userSql.GetUserByID(UserID);
        //    return user.Decks().Count;
        //}
    }
}

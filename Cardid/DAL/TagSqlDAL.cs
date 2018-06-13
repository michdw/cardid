using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Cardid.Models;
using Dapper;

namespace Cardid.DAL
{
    public class TagSqlDAL
    {
        private string connectionString;
        public TagSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private string addTagToDeck = "INSERT INTO [deck_tag] (DeckID, TagID) VALUES (@deckID, @tagID)";
        private string createTag = "INSERT INTO [tags] (TagName, UserID) VALUES (@tagName, @userID)";
        private string getAllTags = "SELECT * FROM [tags] ORDER BY TagName DESC";
        private string getDecksByTagID = "SELECT * FROM [decks] JOIN [deck_tag] on deck_tag.DeckID = decks.DeckID WHERE deck_tag.TagID = @tagID";
        private string getTagByID = "SELECT * FROM [tags] WHERE TagID = @tagID";
        private string getTagsByDeckID = "SELECT * FROM [tags] JOIN [deck_tag] ON deck_tag.TagID = tags.TagID WHERE deck_tag.DeckID = @deckID";
        private string getTagsByUserID = "SELECT * FROM [tags] WHERE UserID = @userID";
        private string removeTag = "DELETE FROM [tags] WHERE TagID = @tagID";
        private string removeTagFromDeck = "DELETE FROM [deck_tag] WHERE DeckID = @deckID AND TagID = @tagID";
        private string removeTagFromAllDecks = "DELETE FROM [deck_tag] WHERE TagID = @tagID";
        private string tagsNotWithDeck = "SELECT * FROM [tags] WHERE TagID NOT IN "
            + "(SELECT TagID FROM [deck_tag] WHERE deck_tag.DeckID = @deckID)";


        public void AddTagToDeck(string deckID, string tagID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(addTagToDeck, new { deckID, tagID });
            }
        }


        public Tag CreateTag(string tagName, string userID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(createTag, new { tagName, userID });
                List<Tag> userTags = db.Query<Tag>(getTagsByUserID, new { userID }).ToList();
                Tag newTag = userTags.Last();
                newTag.TagName = newTag.TagName.Trim();
                return newTag;
            }
        }


        public void DeleteTag(string tagID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(removeTagFromAllDecks, new { tagID });
                db.Execute(removeTag, new { tagID });
            }
        }


        public List<Tag> GetAllTags()
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<Tag> list = db.Query<Tag>(getAllTags).ToList();
                foreach (Tag tag in list)
                {
                    tag.TagName = tag.TagName.Trim();
                }
                return list;
            }
        }


        public Tag GetTagByID(string tagID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<Tag> list = db.Query<Tag>(getTagByID, new { tagID }).ToList();
                Tag tag = list.FirstOrDefault();
                tag.TagName = tag.TagName.Trim();
                return tag;
            }
        }


        public List<Tag> GetTagsByDeckID(string deckID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<Tag> list = db.Query<Tag>(getTagsByDeckID, new { deckID }).ToList();
                foreach (Tag tag in list)
                {
                    tag.TagName = tag.TagName.Trim();
                }
                return list;
            }
        }


        public List<Deck> GetDecksByTagID(string tagID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<Deck> list = db.Query<Deck>(getDecksByTagID, new { tagID }).ToList();
                foreach (Deck deck in list)
                {
                    deck.TrimValues();
                }
                return list;
            }
        }


        public void RemoveTagFromDeck(string deckID, string tagID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(removeTagFromDeck, new { deckID, tagID });
            }
        }


        public List<Tag> TagsNotWithDeck(string deckID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<Tag> list = db.Query<Tag>(tagsNotWithDeck, new { deckID }).ToList();
                foreach (Tag tag in list)
                {
                    tag.TagName = tag.TagName.Trim();
                }
                return list;
            }
        }

    }
}
﻿using System;
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
        readonly string connectionString;
        public TagSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        readonly string addTagToDeck = "INSERT INTO [deck_tag] (DeckID, TagID) VALUES (@deckID, @tagID)";
        readonly string createTag = "INSERT INTO [tags] (TagName, UserID) VALUES (@tagName, @userID)";
        readonly string getAllTagsByName = "SELECT * FROM [tags] ORDER BY TagName ASC";
        readonly string getAllTagsByPopularity = "SELECT tags.TagID FROM [tags] "
            + "FULL OUTER JOIN [deck_tag] ON deck_tag.TagID = tags.TagID "
            + "GROUP BY tags.TagID ORDER BY COUNT(DeckID) DESC, TagID DESC";
        readonly string getOtherTagsByName = "SELECT * FROM [tags] WHERE TagID NOT IN "
            + "(SELECT TagID FROM [deck_tag] WHERE deck_tag.DeckID = @deckID) ORDER BY tags.TagName ASC";
        readonly string getOtherTagsByPopularity = "SELECT tags.TagID FROM[tags] "
            + "FULL OUTER JOIN[deck_tag] ON deck_tag.TagID = tags.TagID "
            + "WHERE tags.TagID NOT IN(SELECT TagID FROM[deck_tag] WHERE deck_tag.DeckID = @deckID) "
            + "GROUP BY tags.TagID ORDER BY COUNT(DeckID) DESC, TagID DESC";
        readonly string getTagByID = "SELECT * FROM [tags] WHERE TagID = @tagID";
        readonly string getTagsByDeckID = "SELECT * FROM [tags] JOIN [deck_tag] ON deck_tag.TagID = tags.TagID "
            + "WHERE deck_tag.DeckID = @deckID ORDER BY tags.TagName DESC";
        readonly string getTagsByCreatorID = "SELECT * FROM [tags] WHERE UserID = @userID";
        readonly string removeTag = "DELETE FROM [tags] WHERE TagID = @tagID";
        readonly string removeTagFromDeck = "DELETE FROM [deck_tag] WHERE DeckID = @deckID AND TagID = @tagID";
        readonly string removeTagFromAllDecks = "DELETE FROM [deck_tag] WHERE TagID = @tagID";
        readonly string searchTagsByName = "SELECT * FROM tags WHERE TagName LIKE @text";


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
                List<Tag> userTags = db.Query<Tag>(getTagsByCreatorID, new { userID }).ToList();
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


        public List<Tag> GetAllTagsByName()
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<Tag> list = db.Query<Tag>(getAllTagsByName).ToList();
                foreach (Tag tag in list)
                {
                    tag.TagName = tag.TagName.Trim();
                }
                return list;
            }
        }


        public List<Tag> GetAllTagsByPopularity()
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<string> list = db.Query<string>(getAllTagsByPopularity).ToList();
                List<Tag> result = new List<Tag>();
                foreach (string tagID in list)
                {
                    List<Tag> tagList = db.Query<Tag>(getTagByID, new { tagID }).ToList();
                    Tag tag = tagList.FirstOrDefault();
                    tag.TagName = tag.TagName.Trim();
                    result.Add(tag);
                }
                return result;
            }
        }


        public List<Tag> GetOtherTagsByName(string deckID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<Tag> list = db.Query<Tag>(getOtherTagsByName, new { deckID }).ToList();
                foreach (Tag tag in list)
                {
                    tag.TagName = tag.TagName.Trim();
                }
                return list;
            }
        }


        public List<Tag> GetOtherTagsByPopularity(string deckID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<string> list = db.Query<string>(getOtherTagsByPopularity, new { deckID }).ToList();
                List<Tag> result = new List<Tag>();
                foreach (string tagID in list)
                {
                    List<Tag> tagList = db.Query<Tag>(getTagByID, new { tagID }).ToList();
                    Tag tag = tagList.FirstOrDefault();
                    tag.TagName = tag.TagName.Trim();
                    result.Add(tag);
                }
                return result;
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


        public List<Tag> GetTagsByCreatorID(string userID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<Tag> list = db.Query<Tag>(getTagsByCreatorID, new { userID }).ToList();
                foreach (Tag tag in list)
                {
                    tag.TagName = tag.TagName.Trim();
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


        public List<Tag> SearchTagsByName(string text)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<Tag> list = db.Query<Tag>(searchTagsByName, new { text = "%" + text + "%" }).ToList();
                return list;
            }
        }

    }
}
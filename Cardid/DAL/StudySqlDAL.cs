using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Cardid.Models;
using Dapper;

namespace Cardid.DAL
{
    public class StudySqlDAL
    {
        private string connectionString;
        public StudySqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private string getSessionsByUserID = "SELECT * FROM [sessions] WHERE UserID = @userID order by TimeOf DESC";
        private string logStudySession = "INSERT INTO [sessions] (DeckID, UserID, TotalScore, PossibleScore, TimeOf) "
            + "VALUES (@deckID, @userID, @totalScore, @possibleScore, @timeOf)";
        private string mostActiveDecks = "SELECT DeckID, COUNT(DeckID) AS Count FROM[sessions] "
            + "GROUP BY DeckID ORDER BY COUNT(DeckID) DESC";
        private string mostActiveUsers = "SELECT UserID, COUNT(UserID) AS Count FROM[sessions] "
            + "GROUP BY UserID ORDER BY COUNT(UserID) DESC";


        public List<Study> GetSessionsByUserID(string userID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Study>(getSessionsByUserID, new { userID }).ToList();
            }
        }

        public Dictionary<int, int> MostActiveDecks()
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                return db.Query(mostActiveDecks).ToDictionary(
                    row => (int)row.DeckID,
                    row => (int)row.Count);
            }
        }

        public Dictionary<int, int> MostActiveUsers()
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                return db.Query(mostActiveUsers).ToDictionary(
                    row => (int)row.UserID,
                    row => (int)row.Count);
            }
        }

        public void LogStudySession(Study study)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(logStudySession, new
                {
                    deckID = study.DeckID,
                    userID = study.UserID,
                    totalScore = study.TotalScore,
                    possibleScore = study.PossibleScore,
                    timeOf = study.TimeOf
                });
            }
        }

    }
}
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

        private string getSessionsByUserID = "SELECT * FROM [sessions] WHERE UserID = @userID";
        private string logStudySession = "INSERT INTO [sessions] (DeckID, UserID, TotalScore, PossibleScore, TimeOf) "
            + "VALUES (@deckID, @userID, @totalScore, @possibleScore, @timeOf)";
        private string getDecksByActivity = "SELECT DeckID, COUNT(DeckID) AS Count FROM[sessions] "
            + "GROUP BY DeckID ORDER BY COUNT(DeckID) DESC";
        private string getUsersByActivity = "SELECT UserID, COUNT(UserID) AS Count FROM[sessions] "
            + "GROUP BY UserID ORDER BY COUNT(UserID) DESC";



        public Dictionary<string, int> GetDecksByActivity()
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                return db.Query(getDecksByActivity).ToDictionary(
                    row => (string)row.DeckID.ToString(),
                    row => (int)row.Count);
            }
        }

        public Dictionary<string, int> GetUsersByActivity()
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                return db.Query(getUsersByActivity).ToDictionary(
                    row => (string)row.UserID.ToString(),
                    row => (int)row.Count);
            }
        }

        public List<Study> GetSessionsByUserID(string userID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Study>(getSessionsByUserID, new { userID }).ToList();
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
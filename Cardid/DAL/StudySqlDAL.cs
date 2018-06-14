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

        private string logStudySession = "INSERT INTO [sessions] (DeckID, UserID, TotalScore, PossibleScore, TimeOf) "
            + "VALUES (@deckID, @userID, @totalScore, @possibleScore, @timeOf)";





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
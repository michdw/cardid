using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Cardid.Models;
using Dapper;

namespace Cardid.DAL
{
    public class UserSqlDAL
    {
        private string connectionString;
        public UserSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private string checkForName = "SELECT * FROM [users] WHERE DisplayName = @name;";
        private string getAllUsers = "SELECT * FROM [users]";
        private string getUserByID = "SELECT * FROM [users] WHERE UserID = @userID";
        private string getUserByEmail = "SELECT * FROM [users] WHERE Email = @email;";
        private string registerUser = "INSERT INTO [users] (Email, Password, DisplayName) VALUES (@email, @password, @displayName);";
        private string updateEmail = "UPDATE [users] SET Email = @email WHERE UserID = @userID";
        private string updateName = "UPDATE [users] SET DisplayName = @displayName WHERE UserID = @userID";
        private string updatePassword = "UPDATE [users] SET Password = @password WHERE UserID = @userID";

        //steps to remove user account
        private string removeUserCardDecks = "DELETE [card_deck] from [card_deck] join [cards] on card_deck.CardID = cards.CardID WHERE cards.UserID = @userID; ";
        private string removeUserCards = "DELETE FROM [cards] WHERE UserID = @userID; ";
        private string removeUserDecks = "DELETE FROM [decks] WHERE UserID = @userID; ";
        private string removeUserSessions = "DELETE FROM [sessions] WHERE UserID = @userID; ";
        private string removeUser = "DELETE FROM [users] WHERE UserID = @userID;";


        public bool CheckForEmail(string email)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<User> list = db.Query<User>(getUserByEmail, new { email }).ToList();
                return list.Count > 0;
            }
        }


        public bool CheckForName(string name)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<User> list = db.Query<User>(checkForName, new { name }).ToList();
                return list.Count > 0;
            }
        }


        public List<User> GetAllUsers()
        {
            List<User> list = new List<User>();

            return list;
        }


        public User GetUserByID(string userID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                return db.Query<User>(getUserByID, new { userID }).ToList().FirstOrDefault<User>().TrimValues();
            }
        }


        public User GetUserByEmail(string email)
        {
            User nullUser = new User();
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<User> users = db.Query<User>(getAllUsers).ToList();
                foreach (User user in users)
                {
                    user.TrimValues();
                    if (user.Email == email)
                    {
                        return user;
                    }
                }
                return nullUser;
            }
        }


        public void RegisterUser(User user)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(registerUser, new { email = user.Email, password = user.Password, displayName = user.DisplayName });
            }
        }


        public void RemoveUser(string userID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(removeUserCardDecks, new { userID });
                db.Execute(removeUserCards, new { userID });
                db.Execute(removeUserDecks, new { userID });
                db.Execute(removeUserSessions, new { userID });
                db.Execute(removeUser, new { userID });
            }
        }


        public void UpdateEmail(string email, string userID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(updateEmail, new { email, userID });
            }
        }


        public void UpdateName(string displayName, string userID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(updateName, new { displayName, userID });
            }
        }


        public void UpdatePassword(string password, string userID)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(updatePassword, new { password, userID });
            }
        }

    }
}
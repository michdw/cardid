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
        private string checkForEmail = "SELECT * from [users] WHERE Email = @email";
        private string getAllUsers = "SELECT * FROM [users]";
        private string getUserByID = "SELECT UserID, Email, Password, DisplayName FROM [users] where UserID = @userID";
        private string getUserInfo = "SELECT UserID, Email, Password, DisplayName FROM [users] WHERE Email = @email;";
        private string registerUser = "INSERT INTO [users] (Email, Password, DisplayName) VALUES (@email, @password, @displayName);";

        public UserSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }


        public bool CheckForEmail(string email)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<User> list = db.Query<User>(checkForEmail, new { email }).ToList();
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


        public User GetUserInfo(string email)
        {
            User user = new User();
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                List<User> users = db.Query<User>(getUserInfo, new { email }).ToList();
                if (users.Count > 0)
                {
                    user = users.FirstOrDefault<User>().TrimValues();
                }
                return user;
            }
        }


        public void RegisterUser(User user)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Execute(registerUser, new { email = user.Email, password = user.Password, displayName = user.DisplayName });
            }
        }

    }
}
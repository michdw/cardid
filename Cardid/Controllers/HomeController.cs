using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cardid.DAL;
using Cardid.Models;

namespace Cardid.Controllers
{
    public class HomeController : Controller
    {
        DeckSqlDAL deckSql = new DeckSqlDAL(ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString);
        StudySqlDAL studySql = new StudySqlDAL(ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString);
        TagSqlDAL tagSql = new TagSqlDAL(ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString);
        UserSqlDAL userSql = new UserSqlDAL(ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString);

        private string GetUser()
        {
            return Session["userid"].ToString();
        }



        public ActionResult Index()
        {
            Session["anon"] = "Home";

            Dictionary<int, int> activeUsersSql = studySql.MostActiveUsers();
            Dictionary<string, int> activeUsers = new Dictionary<string, int>();
            foreach (KeyValuePair<int, int> kvp in activeUsersSql)
            {
                User user = userSql.GetUserByID(kvp.Key.ToString());
                string name = user.DisplayName;
                activeUsers.Add(name, kvp.Value);
            }

            Dictionary<int, int> activeDecksSql = studySql.MostActiveDecks();
            Dictionary<Deck, int> activeDecks = new Dictionary<Deck, int>();
            foreach (KeyValuePair<int, int> kvp in activeDecksSql)
            {
                Deck deck = deckSql.GetDeckByID(kvp.Key.ToString());
                if (deck.IsPublic)
                {
                    activeDecks.Add(deck, kvp.Value);
                }
            }

            List<Tag> popularTagsSql = tagSql.GetAllTagsByPopularity();
            Dictionary<Tag, int> popularTags = new Dictionary<Tag, int>();
            foreach (Tag tag in popularTagsSql)
            {
                int decksUsing = tag.Decks().Count;
                popularTags.Add(tag, decksUsing);
            }


            Stats stats = new Stats
            {
                ActiveDecks = activeDecks,
                ActiveUsers = activeUsers,
                PopularTags = popularTags,
            };

            return View(stats);
        }


        //login actions
        public ActionResult Login()
        {
            User model = new User();

            return View("Login");
        }


        [HttpPost]
        public ActionResult CompleteLogin(User user)
        {
            if (user.Email == null || user.Password == null)
            {
                ModelState.AddModelError("invalid-credentials", "Please enter an email and password.");
                return View("Login", user);
            }

            user.Email = user.Email.ToLower();
            User currentUser = userSql.GetUserByEmail(user.Email);

            if (currentUser.Email == null)
            {
                ModelState.AddModelError("invalid-credentials", "That email isn't currently registered.");
                ViewBag.RegisterInstead = true;
                return View("Login", user);
            }
            else if (currentUser.Password != user.Password)
            {
                ModelState.AddModelError("invalid-credentials", "Incorrect password");
                return View("Login", user);
            }

            Session["userid"] = currentUser.UserID;
            Session["username"] = currentUser.DisplayName;
            TempData["user-login"] = currentUser;

            switch (Session["anon"].ToString())
            {
                case "Card":
                    return RedirectToAction("Index", "Card");
                case "Deck":
                    return RedirectToAction("Index", "Deck");
                default:
                    return RedirectToAction("Index");
            }
        }


        //registration actions
        public ActionResult BeginRegister()
        {
            User model = new User();

            return View("Register", model);
        }


        [HttpPost]
        public ActionResult CompleteRegister(User newUser)
        {
            newUser.Email = newUser.Email.ToLower();
            bool emailExists = userSql.CheckForEmail(newUser.Email);
            bool nameExists = userSql.CheckForName(newUser.DisplayName);
            if (emailExists)
            {
                ModelState.AddModelError("invalid-credentials", "That email has already been used to register on this site.");
                ViewBag.LoginInstead = true;
                return View("Register", newUser);
            }
            else if (nameExists)
            {
                ModelState.AddModelError("invalid-credentials", "That name is already in use by a different user.");
                ViewBag.LoginInstead = true;
                return View("Register", newUser);
            }

            if (!ModelState.IsValid)
            {
                return View("Register", newUser);
            }

            userSql.RegisterUser(newUser);
            User currentUser = userSql.GetUserByEmail(newUser.Email);

            Session["userid"] = currentUser.UserID;
            Session["username"] = currentUser.DisplayName;
            TempData["new-user"] = currentUser.DisplayName;
            switch (Session["anon"].ToString())
            {
                case "Card":
                    return RedirectToAction("Index", "Card");
                case "Deck":
                    return RedirectToAction("Index", "Deck");
                default:
                    return RedirectToAction("Index");
            }
        }


        //other actions
        public ActionResult Account()
        {
            string userID = GetUser();
            User user = userSql.GetUserByID(userID);

            return View(user);
        }


        public ActionResult About()
        {
            return View();
        }


        public ActionResult ChangeInfoInit()
        {
            string userID = GetUser();
            User user = userSql.GetUserByID(userID);

            return View("ChangeUserInfo", user);
        }


        [HttpPost]
        public ActionResult ChangeName(User user)
        {
            string userID = GetUser();
            User oldInfo = userSql.GetUserByID(userID);
            bool nameExists = userSql.CheckForName(user.DisplayName);

            var displayName = ModelState["DisplayName"];
            if (displayName == null || displayName.Errors.Any())
            {
                TempData["change-result"] = "Invalid Input: Your name hasn't been changed.";
                return View("ChangeUserInfo", oldInfo);
            }
            else if (nameExists)
            {
                TempData["change-result"] = "Invalid input: That name is already in use by a different user.";
                return View("ChangeUserInfo", oldInfo);
            }
            userSql.UpdateName(user.DisplayName, userID);
            user = userSql.GetUserByID(userID);

            TempData["change-result"] = "Name changed successfully";
            return RedirectToAction("ChangeInfoInit", user);
        }


        [HttpPost]
        public ActionResult ChangeEmail(User user)
        {
            user.Email = user.Email.ToLower();
            string userID = GetUser();
            User oldInfo = userSql.GetUserByID(userID);
            bool emailExists = userSql.CheckForEmail(user.Email);

            var userEmail = ModelState["Email"];
            if (userEmail == null || userEmail.Errors.Any())
            {
                TempData["change-result"] = "Invalid input: Your email hasn't been changed.";
                return View("ChangeUserInfo", oldInfo);
            }
            else if (emailExists)
            {
                TempData["change-result"] = "Invalid input: That email is associated with a different user.";
                return View("ChangeUserInfo", oldInfo);
            }

            userSql.UpdateEmail(user.Email, userID);
            user = userSql.GetUserByID(userID);

            TempData["change-result"] = "Email changed successfully";
            return RedirectToAction("ChangeInfoInit", user);
        }


        [HttpPost]
        public ActionResult ChangePassword(User user)
        {
            string userID = GetUser();
            User oldInfo = userSql.GetUserByID(userID);

            var password = ModelState["Password"];
            var confirmPassword = ModelState["ConfirmPassword"];
            if (password == null || password.Errors.Any())
            {
                TempData["change-result"] = "Invalid input: your password hasn't been changed.";
                return View("ChangeUserInfo", oldInfo);
            }
            else if (confirmPassword == null)
            {
                TempData["change-result"] = "Please enter your password twice; password hasn't been changed.";
                return View("ChangeUserInfo", oldInfo);
            }
            else if (confirmPassword.Errors.Any())
            {
                TempData["change-result"] = "Passwords didn't match; password hasn't been changed.";
                return View("ChangeUserInfo", oldInfo);
            }
            userSql.UpdatePassword(user.Password, userID);
            user = userSql.GetUserByID(userID);

            TempData["change-result"] = "Password changed successfully";
            return RedirectToAction("ChangeInfoInit", user);
        }


        [HttpPost]
        public ActionResult RemoveUser(string userID)
        {
            List<Tag> userTags = tagSql.GetTagsByUserID(userID);
            foreach (Tag tag in userTags)
            {
                if (tag.CurrentUserIDs().Count <= 1)
                {
                    tagSql.DeleteTag(tag.TagID);
                }
            }

            userSql.RemoveUser(userID);
            return RedirectToAction("Logout");
        }


        public ActionResult Logout()
        {
            TempData["login-name"] = null;
            Session["userid"] = null;
            Session["username"] = null;
            Session["currentdeck"] = null;
            Session["anon"] = "Home";
            ViewBag.LoggedOut = true;
            return View("Index");
        }
    }
}
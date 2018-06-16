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
        UserSqlDAL userSql = new UserSqlDAL(ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString);
        TagSqlDAL tagSql = new TagSqlDAL(ConfigurationManager.ConnectionStrings["FlashCardsDB"].ConnectionString);

        private string GetUser()
        {
            return Session["userid"].ToString();
        }



        public ActionResult Index()
        {
            Session["anon"] = "Home";

            return View();
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
            TempData["loginname"] = currentUser.DisplayName;
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
            bool emailExists = userSql.CheckForEmail(newUser.Email);
            if (emailExists)
            {
                ModelState.AddModelError("invalid-credentials", "That email has already been used to register on this site.");
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
            TempData["newuser"] = currentUser.DisplayName;
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
        public ActionResult UserPage()
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

            var displayName = ModelState["DisplayName"];
            if (displayName == null || displayName.Errors.Any())
            {
                TempData["result"] = "Invalid Input: Your name hasn't been changed.";
                return View("ChangeUserInfo", oldInfo);
            }
            userSql.UpdateName(user.DisplayName, userID);
            user = userSql.GetUserByID(userID);

            TempData["result"] = "Name changed successfully";
            return RedirectToAction("ChangeInfoInit", user);
        }


        [HttpPost]
        public ActionResult ChangeEmail(User user)
        {
            string userID = GetUser();
            User oldInfo = userSql.GetUserByID(userID);

            var userEmail = ModelState["Email"];
            if (userEmail == null || userEmail.Errors.Any())
            {
                TempData["result"] = "Invalid Input: Your email hasn't been changed.";
                return View("ChangeUserInfo", oldInfo);
            }
            userSql.UpdateEmail(user.Email, userID);
            user = userSql.GetUserByID(userID);

            TempData["result"] = "Email changed successfully";
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
                TempData["result"] = "Invalid input: your password hasn't been changed.";
                return View("ChangeUserInfo", oldInfo);
            }
            else if (confirmPassword == null) {
                TempData["result"] = "Please enter your password twice; password hasn't been changed.";
                return View("ChangeUserInfo", oldInfo);
            }
            else if (confirmPassword.Errors.Any())
            {
                TempData["result"] = "Passwords didn't match; password hasn't been changed.";
                return View("ChangeUserInfo", oldInfo);
            }
            userSql.UpdatePassword(user.Password, userID);
            user = userSql.GetUserByID(userID);

            TempData["result"] = "Password changed successfully";
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
            TempData["loginname"] = null;
            Session["userid"] = null;
            Session["username"] = null;
            Session["currentdeck"] = null;
            Session["anon"] = "Home";
            return View("Index");
        }
    }
}
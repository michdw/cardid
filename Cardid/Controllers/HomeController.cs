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

            User currentUser = userSql.GetUserInfo(user.Email);

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

            if (!ModelState.IsValid)
            {
                return View("Register", newUser);
            }

            //check for duplicate email???

            userSql.RegisterUser(newUser);
            User currentUser = userSql.GetUserInfo(newUser.Email);

            Session["userid"] = currentUser.UserID;
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


        //
        public ActionResult Logout()
        {
            TempData["loginname"] = null;
            Session["userid"] = null;
            Session["anon"] = "Home";
            return View("Index");
        }

    }
}
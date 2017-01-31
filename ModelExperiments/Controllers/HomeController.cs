using ModelExperiments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Core.Objects;

namespace ModelExperiments.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext dB = new ApplicationDbContext();

        public ActionResult Index()
        {
            var LoggedInUser = User.Identity.GetUserName();
            //ViewBag.IsMail =
            var query =
            
                   dB.ForumPosts.Any(f => f.IsPrivate == true && f.IsRead == false && f.PmToUserName == LoggedInUser);

            if (query)
            {
                Session["HasMail"] = true;
            }
            else
            {
                Session["HasMail"] = false;
            }
            //var defaultVote = false;
            //ViewBag.Mail = (dB.ForumPosts
            //   .Where(f => f.IsPrivate == true && f.IsRead == false)
            //   .FirstOrDefault());
            //ViewBag.Mail = true;
            return View();
        }

     

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
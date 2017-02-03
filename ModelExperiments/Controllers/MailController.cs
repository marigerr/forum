using Microsoft.AspNet.Identity;
using ModelExperiments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ModelExperiments.Controllers
{
    public class MailController : Controller
    {
        private ApplicationDbContext dB = new ApplicationDbContext();

        public ActionResult Mail()
        {
            return View();
        }

        [Authorize]
        public JsonResult CheckMail()
        {
          var LoggedInUser = User.Identity.GetUserName();
          var query =

                 dB.ForumPosts.Any(f => f.IsPrivate == true && f.IsRead == false && f.PmToUserName == LoggedInUser);

          if (query)
          {
            return Json(true, JsonRequestBehavior.AllowGet);
          }
          else
          {
            return Json(false, JsonRequestBehavior.AllowGet);
          }
        }

        [Authorize]
        public JsonResult ReadPrivateMessage()
        {
            ForumListViewModel result = new ForumListViewModel();

            result.LoggedInUser = User.Identity.GetUserName();
            result.ForumPosts = dB.ForumPosts
                    .Where(f => f.IsPrivate == true && f.PmToUserName == result.LoggedInUser)
                    .Select(item => new ForumPostDTO
                    {
                        PostId = item.PostId,
                        Title = item.Title,
                        PostContent = item.PostContent,
                        UserName = item.User.UserName,
                        DateJoined = item.User.DateJoined,
                        TimeStamp = item.TimeStamp,
                        IsRead = item.IsRead
                    })
                    .OrderByDescending(f => f.TimeStamp)
                    .ToList();

            return Json(result.ForumPosts, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult MarkReadPM(int postId)
        {
            ForumPost currentPM = new ForumPost();
            currentPM = dB.ForumPosts.Find(postId);
            currentPM.IsRead = true;
            dB.SaveChanges();

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

            return RedirectToAction("ReadPrivateMessage");
        }

        

        [Authorize]
        public ActionResult MarkUnReadPM(int postId)
        {
            ForumPost currentPM = new ForumPost();
            currentPM = dB.ForumPosts.Find(postId);
            currentPM.IsRead = false;
            dB.SaveChanges();

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

            return RedirectToAction("ReadPrivateMessage");
        }

        [Authorize]
        public ActionResult DeletePM(int postId)
        {
            dB.ForumPosts.RemoveRange(dB.ForumPosts.Where(f => f.PostId == postId || f.ParentPostId == postId));
            dB.SaveChanges();

            return RedirectToAction("ReadPrivateMessage");
        }
    }
}
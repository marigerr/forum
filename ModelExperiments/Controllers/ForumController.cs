using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ModelExperiments.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ModelExperiments.Controllers
{

    [Authorize]
    public class ForumController : Controller
    {
        private ApplicationDbContext dB = new ApplicationDbContext();

        private ApplicationUserManager _userManager;

        public ForumController()
        {
        }

        public ForumController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [Authorize]
        public ActionResult Forum()
        {
            return View();
        }

        [Authorize]
        public JsonResult ForumList()
        {
            ForumListViewModel result = new ForumListViewModel();

            var forumPosts = dB.ForumPosts.Include(f => f.User)
                .Where(f => f.ParentPostId == null && f.IsPrivate != true)
                .Select(item => new ForumPostDTO
                {
                    PostId = item.PostId,
                    Title = item.Title,
                    PostContent = item.PostContent,
                    UserName = item.User.UserName,
                    DateJoined = item.User.DateJoined,
                    TimeStamp = item.TimeStamp
                })
                .OrderByDescending(f => f.TimeStamp);

            result.ForumPosts = forumPosts.ToList();
            var user = UserManager.FindById(User.Identity.GetUserId());
            result.LoggedInUser = User.Identity.GetUserName();
            result.LoggedInUserStatus =user.Status;
            result.IsAdminOrMod = User.IsInRole("Admin") || User.IsInRole("Moderator");

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult CreateOrUpdatePost(ForumPost newPost)
        {
            newPost.UserId = User.Identity.GetUserId();
            newPost.TimeStamp = DateTime.Now;
            if (newPost.PostId == 0)
            {
                dB.ForumPosts.Add(newPost); 
            }
            else
            {
                dB.Entry(newPost).State = EntityState.Modified;
            }
            dB.SaveChanges();

            return RedirectToAction("ForumList");
        }

        public ActionResult DeletePost(int postId)
        {
            dB.ForumPosts.RemoveRange(dB.ForumPosts.Where(f => f.PostId == postId || f.ParentPostId == postId));
            dB.SaveChanges();

            return RedirectToAction("ForumList");
        }

        public JsonResult GetForumPostToEdit(int postId)
        {
            var result = dB.ForumPosts
                .Where(f => f.PostId == postId)
                 .Select(item => new {
                     item.PostId,
                     item.Title,
                     item.PostContent,
                 })
                 .FirstOrDefault(); 
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserPosts(string userName)
        {
            ForumListViewModel result = new ForumListViewModel();

            result.ForumPosts = dB.ForumPosts.Include(f => f.User)
                .Where(f => f.User.UserName == userName && f.IsPrivate != true)
                .Select(item => new ForumPostDTO
                {
                    PostId = item.PostId,
                    ParentPostId = item.ParentPostId,
                    Title = item.Title,
                    PostContent = item.PostContent,
                    UserName = item.User.UserName,
                    DateJoined = item.User.DateJoined,
                    TimeStamp = item.TimeStamp
                })
                .OrderByDescending(f => f.TimeStamp)
                .ToList();

            result.LoggedInUser = User.Identity.GetUserName();
            result.ProfileUser = userName;

            result.ProfileUserDateJoined = dB.Users.Where(u => u.UserName == userName).Select(u => u.DateJoined).FirstOrDefault(); ;

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult CreateOrUpdatePM(ForumPost newPM)
        {
            newPM.TimeStamp = DateTime.Now;
            newPM.UserId = User.Identity.GetUserId();
            newPM.IsPrivate = true;
            newPM.IsRead = false;

            if (newPM.PostId == 0)
            {
                dB.ForumPosts.Add(newPM);
            }
            else
            {
                dB.Entry(newPM).State = EntityState.Modified;
            }
            dB.SaveChanges();

            var result = true;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult ReadPostAndReplies(int postId)
        {
            ThreadReplyViewModel result = new ThreadReplyViewModel();
            result.forumPost = dB.ForumPosts.Find(postId);
            var replies = dB.ForumPosts
                .Include(f => f.User)
                .Where(f => f.ParentPostId == postId)
                .Select(item => new ForumPostDTO()
                {
                    PostId = item.PostId,
                    ParentPostId = item.ParentPostId,
                    Title = item.Title,
                    PostContent = item.PostContent,
                    UserName = item.User.UserName,
                    DateJoined = item.User.DateJoined,
                    TimeStamp = item.TimeStamp
                })
                .OrderByDescending(f => f.TimeStamp);

            result.ForumPostReplies = replies.ToList();
            result.LoggedInUser = User.Identity.GetUserName();
            var user = UserManager.FindById(User.Identity.GetUserId());
            result.LoggedInUserStatus = user.Status;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ReplyPost(ForumPost newReply)
        {
            newReply.UserId = User.Identity.GetUserId();
            newReply.TimeStamp = DateTime.Now;
            dB.ForumPosts.Add(newReply);
            dB.SaveChanges();

            return RedirectToAction("ReadPostAndReplies", new { postId = newReply.ParentPostId });
        }

 


        ////////////////////////////////////////////////////////////////////////
        // GET: Forum
        public ActionResult OldIndex()
        {
            var forumPosts = dB.ForumPosts.Include(f => f.User.UserName);
            return View(forumPosts.ToList());
        }

        // GET: Forum/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumPost forumPost = dB.ForumPosts.Find(id);
            if (forumPost == null)
            {
                return HttpNotFound();
            }
            return View(forumPost);
        }

        // GET: Forum/Create
        public ActionResult Create()
        {
            ViewBag.UserId = User.Identity.GetUserId();
            //ViewBag.UserId = new SelectList(dB.ApplicationUsers, "Id", "FirstName");
            return View();
        }

        // POST: Forum/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,ParentPostId,PostContent,UserId")] ForumPost forumPost)
        {
            if (ModelState.IsValid)
            {
                forumPost.TimeStamp = DateTime.Now;
                dB.ForumPosts.Add(forumPost);
                dB.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.UserId = new SelectList(dB.AspNetUsers, "Id", "FirstName", forumPost.UserId);
            return View(forumPost);
        }

        // GET: Forum/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumPost forumPost = dB.ForumPosts.Find(id);
            if (forumPost == null)
            {
                return HttpNotFound();
            }
            //ViewBag.UserId = new SelectList(dB.ApplicationUsers, "Id", "FirstName", forumPost.UserId);
            return View(forumPost);
        }

        // POST: Forum/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,ParentPostId,PostContent,TimeStamp,UserId")] ForumPost forumPost)
        {
            if (ModelState.IsValid)
            {
                dB.Entry(forumPost).State = EntityState.Modified;
                dB.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.UserId = new SelectList(dB.ApplicationUsers, "Id", "FirstName", forumPost.UserId);
            return View(forumPost);
        }

        // GET: Forum/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumPost forumPost = dB.ForumPosts.Find(id);
            if (forumPost == null)
            {
                return HttpNotFound();
            }
            return View(forumPost);
        }

        // POST: Forum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ForumPost forumPost = dB.ForumPosts.Find(id);
            dB.ForumPosts.Remove(forumPost);
            dB.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dB.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

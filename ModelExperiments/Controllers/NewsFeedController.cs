using ModelExperiments.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ModelExperiments.Controllers
{
    public class NewsFeedController : Controller
    {
        private ApplicationDbContext dB = new ApplicationDbContext();

        public ActionResult NewsFeed()
        {
            return View();
        }

        // GET: NewsFeed
        public JsonResult NewsFeedIndex()
        {
            NewsFeedViewModel result = new NewsFeedViewModel();
            result.NewsItems = dB.NewsFeedItems
                .Select(item => new NewsItemDTO
                {
                    ItemId = item.ItemId,
                    Title = item.Title,
                    ItemContent = item.ItemContent,
                    TimeStamp = item.TimeStamp,
                    EventDateTime = item.EventDateTime,
                    Organizer = item.Organizer,
                    OrganizerContactEmail = item.OrganizerContactEmail,
                    IsEvent = item.IsEvent,
                })
                .OrderByDescending(n => n.TimeStamp).ToList(); 

            result.IsAdminOrMod = User.IsInRole("Admin") || User.IsInRole("Moderator");


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNewsItem(int itemId)
        {
            var result = dB.NewsFeedItems.Find(itemId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin , Moderator")]
        public ActionResult CreateOrUpdateNewsItem(NewsFeedItem newNews)
        {
            //newNews.EventDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            //                                    .AddMilliseconds(1310522400000)
            //                                    .ToLocalTime();
            //if (newNews.IsEvent== true)
            //{
            //    DateTime tempTime = DateTime.Parse(newNews.JSEventDateTime,
            //                  null,
            //                  DateTimeStyles.RoundtripKind);
            //    TimeZone.CurrentTimeZone.ToLocalTime(tempTime);
            //    newNews.EventDateTime = tempTime;
            //}
            if (newNews.IsEvent == true)
            {
                newNews.EventDateTime = DateTime.Parse(newNews.JSEventDateTime);
            }
            newNews.TimeStamp = DateTime.Now;
            if (newNews.ItemId == 0)
            {
                dB.NewsFeedItems.Add(newNews);
            }
            else
            {
                dB.Entry(newNews).State = EntityState.Modified;
            }
            dB.SaveChanges();

            return RedirectToAction("NewsFeedIndex");
        }

        [HttpPost]
        [Authorize(Roles = "Admin , Moderator")]
        public ActionResult DeleteItem(int itemId)
        {
            NewsFeedItem itemToDelete= dB.NewsFeedItems.Single(f => f.ItemId == itemId);
            dB.NewsFeedItems.Remove(itemToDelete);
            dB.SaveChanges();

            return RedirectToAction("NewsFeedIndex");
        }
    }
}
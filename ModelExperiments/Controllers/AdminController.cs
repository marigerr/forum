using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ModelExperiments.Models;
using System.Web.Security;

namespace ModelExperiments.Controllers
{
    [Authorize(Roles = "Admin , Moderator")]
    public class AdminController : Controller
    {
        private ApplicationDbContext dB = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        //private roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dB));

        public AdminController()
        {
        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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
        public ActionResult Admin()
        {
            return View();
        }


        public JsonResult GetUsers()
        {

            List<UserRoleViewModel> userList = new List<UserRoleViewModel>();

            //var users = dB.Users
            //    .Where(x => x.Roles.Select(y => y.RoleId).Contains(UserId))
            //    .ToList();

            userList = UserManager.Users
                 .Select(item => new UserRoleViewModel
                 {
                     UserName = item.UserName,
                     Email = item.Email,
                     DateJoined = item.DateJoined,
                     RoleList = UserManager.GetRoles(item.Id)
                 })
                .ToList();

            //var tempList = UserManager.Users.ToList();
            //userList = tempList
            //     .Select(item => new UserRoleViewModel
            //     {
            //         UserName = item.UserName,
            //         Email = item.Email,
            //         DateJoined = item.DateJoined,
            //         RoleList = UserManager.GetRoles(item.Id)
            //     });


            return Json(userList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetList(string roleString)
        {
            List<ApplicationUser> tempList = new List<ApplicationUser>();
            List<UserRoleViewModel> listToReturn = new List<UserRoleViewModel>();

            var tempUserList = dB.Users.ToList();


            foreach (var user in tempUserList)
            {
                if (UserManager.IsInRole(user.Id, roleString))
                {
                    tempList.Add(user);
                }
            }
            listToReturn = tempList
                .Where(item => item.Status != "Deactivated")
                .Select(item => new UserRoleViewModel
                {
                    UserName = item.UserName,
                    Email = item.Email,
                    DateJoined = item.DateJoined,
                    RoleList = UserManager.GetRoles(item.Id)
                })
                .ToList();

            return Json(listToReturn, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDeactivated()
        {
            List<ApplicationUser> tempList = new List<ApplicationUser>();
            List<UserRoleViewModel> listToReturn = new List<UserRoleViewModel>();

            var tempUserList = dB.Users.ToList();


            //foreach (var user in tempUserList)
            //{
            //    if (UserManager.IsInRole(user.Id, roleString))
            //    {
            //        tempList.Add(user);
            //    }
            //}
            listToReturn = tempUserList
                .Where(item => item.Status == "Deactivated")
                .Select(item => new UserRoleViewModel
                {
                    UserName = item.UserName,
                    Email = item.Email,
                    DateJoined = item.DateJoined,
                    Status = item.Status,
                    RoleList = UserManager.GetRoles(item.Id)
                })
                .ToList();

            return Json(listToReturn, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddRoleToUser(string UserName, string RoleToSet)
        {
            var currentUserId = UserManager.FindByName(UserName).Id;
            var result = UserManager.AddToRole(currentUserId, RoleToSet);

            return RedirectToAction("GetList", new { roleString = "User" });
        }

        public ActionResult RemoveRoleFromUser(string UserName, string RoleToRemove)
        {
            var currentUserId = UserManager.FindByName(UserName).Id;
            var result = UserManager.RemoveFromRole(currentUserId, RoleToRemove);

            return RedirectToAction("GetList", new { roleString = "User" });
        }

        [HttpPost]
        public ActionResult DeactivateUser(string UserName)
        {
            var userToDeactivate = UserManager.FindByName(UserName);
            userToDeactivate.Status = "Deactivated";
            UserManager.Update(userToDeactivate);

            return RedirectToAction("GetList", new { roleString = "User" });
        }

        [HttpPost]
        public ActionResult ActivateUser(string UserName)
        {
            var userToDeactivate = UserManager.FindByName(UserName);
            userToDeactivate.Status = "Active";
            UserManager.Update(userToDeactivate);

            return RedirectToAction("GetList", new { roleString = "User" });
        }
    }
}

        //public ActionResult AddRoleToUser(string Id, string RoleToSet)
        //{
        //    var result = UserManager.AddToRole(Id, RoleToSet);

        //    if (result.Succeeded)
        //    {
        //        ViewBag.msg = RoleToSet + " role was added";
        //        return View("UserList", UserManager.Users.ToList());
        //    }
        //    ViewBag.msg = "Did not work";
        //    return View("UserList", UserManager.Users.ToList());
        //}
        //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
        //roleManager.
        //List<UserRoleViewModel> adminList = new List<UserRoleViewModel>();

        ////var role = roleManager.FindByName(roleName).Users.First();
        ////var usersInRole = Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(role.RoleId)).ToList();

        //var adminNameList = Roles.GetUsersInRole("Admin");
        //adminList = UserManager.Users
        //    .Where(u => adminNameList.Contains(u.UserName))
        //    .Select(item => new UserRoleViewModel
        //          {
        //              UserName = item.UserName,
        //              Email = item.Email,
        //              DateJoined = item.DateJoined
        //          })
        //    .ToList();

        //// GET: Role
        //public ActionResult Index()
        //{
        //    var Roles = dB.Roles.ToList();
        //    return View(Roles);
        //}

        //public ActionResult CreateRole()
        //{
        //    var Role = new IdentityRole();
        //    return View(Role);
        //}

        //[HttpPost]
        //public ActionResult CreateRole(IdentityRole Role)
        //{
        //    dB.Roles.Add(Role);
        //    dB.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //public ActionResult AssignRole()
        //{
        //    return View();
        //}



        //public RoleController()
        //{
        //}

        //public RoleController(ApplicationUserManager userManager)
        //{
        //    Microsoft.AspNet.Identity.UserManager = userManager;
        //}

        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        _userManager = value;
        //    }
        //}
        //public ActionResult UserAndRoleList()
        //{
        //    //var applicationDbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
        //    var users = from u in dB.Users
        //                from ur in u.Roles
        //                join r in dB.Roles on ur.RoleId equals r.Id
        //                select new UserRoleViewModel
        //                {
        //                    UserName = u.UserName,
        //                    Email = u.Email,
        //                    DateJoined = u.DateJoined,
        //                    //RoleList = u.Roles
        //                    //u.Id,
        //                    //Name = u.UserName,
        //                    RoleList = dB.Roles.Where(rl => u.Roles.Select(ur => ur.RoleId).Contains(r.Id)).Select(rl => r.Name).ToList()
        //                };

        //    // users is anonymous type, map it to a Model 
        //    return View(users);
        //}
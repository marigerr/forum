namespace ModelExperiments.Migrations
{
  using Microsoft.AspNet.Identity;
  using Microsoft.AspNet.Identity.EntityFramework;
  using Models;
  using System;
  using System.Data.Entity;
  using System.Data.Entity.Migrations;
  using System.Linq;

  internal sealed class Configuration : DbMigrationsConfiguration<ModelExperiments.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ModelExperiments.Models.ApplicationDbContext context)
      {
        //  This method will be called after migrating to the latest version.

        //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
        //  to avoid creating duplicate seed data. E.g.
        //
        //    context.People.AddOrUpdate(
        //      p => p.FullName,
        //      new Person { FullName = "Andrew Peters" },
        //      new Person { FullName = "Brice Lambson" },
        //      new Person { FullName = "Rowan Miller" }
        //    );
        //

        //context.ForumPosts.AddOrUpdate(x => x.PostId,
        //    new ForumPost()
        //    {
        //        PostId = 1,
        //        Title = "Where is a good place to eat?",
        //        PostContent = "I am in town visiting and looking for recommmendations for restaurants",
        //        UserId = 1,
        //        TimeStamp = new DateTime(2015, 12, 18),
        //    },
        //     new ForumPost()
        //     {
        //         PostId = 1,
        //         Title = "Where is a good place to eat?",
        //         PostContent = "I am in town visiting and looking for recommmendations for restaurants",
        //         UserId = 1,
        //         TimeStamp = new DateTime(2016, 1, 18),
        //     },
        //      new ForumPost()
        //      {
        //          PostId = 1,
        //          Title = "Where is a good place to eat?",
        //          PostContent = "I am in town visiting and looking for recommmendations for restaurants",
        //          UserId = 1,
        //          TimeStamp = new DateTime(2016, 3, 15),
        //      },
        //       new ForumPost()
        //       {
        //           PostId = 1,
        //           Title = "Where is a good place to eat?",
        //           PostContent = "I am in town visiting and looking for recommmendations for restaurants",
        //           UserId = 1,
        //           TimeStamp = new DateTime(2016, 4, 02),
        //       }
        //    );




        //  This method will be called after migrating to the latest version.

        //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
        //  to avoid creating duplicate seed data. E.g.
        //
        //    context.People.AddOrUpdate(
        //      p => p.FullName,
        //      new Person { FullName = "Andrew Peters" },
        //      new Person { FullName = "Brice Lambson" },
        //      new Person { FullName = "Rowan Miller" }
        //    );
        //

        var userStore = new UserStore<ApplicationUser>(context);
        var userManager = new UserManager<ApplicationUser>(userStore);

        var roleStore = new RoleStore<IdentityRole>(context);
        var roleManager = new RoleManager<IdentityRole>(roleStore);

        // check if roles exist if not create them
        if (!roleManager.RoleExists("Admin"))
        {
          //var result = roleManager.Create(new IdentityRole("Admins"));
          roleManager.Create(new IdentityRole("Admin"));
        }
        if (!roleManager.RoleExists("Moderator"))
        {
          roleManager.Create(new IdentityRole("Moderator"));
        }
        if (!roleManager.RoleExists("User"))
        {
          roleManager.Create(new IdentityRole("User"));
        }

        if (userManager.FindByName("SueAdmin") == null)
        {
          var tempuser = new ApplicationUser() { UserName = "SueAdmin", Email = "Sue@fake.com", EmailConfirmed = true, DateJoined = DateTime.Now };
          userManager.Create(tempuser, "Password8!");
          userManager.AddToRole(userManager.FindByName("SueAdmin").Id, "Admin");
          userManager.AddToRole(userManager.FindByName("SueAdmin").Id, "Moderator");
          userManager.AddToRole(userManager.FindByName("SueAdmin").Id, "User");
        }

        if (userManager.FindByName("FredMod") == null)
        {
          var tempuser = new ApplicationUser() { UserName = "FredMod", Email = "Fred@fake.com", EmailConfirmed = true, DateJoined = DateTime.Now };
          userManager.Create(tempuser, "Password9!");
          userManager.AddToRole(userManager.FindByName("FredMod").Id, "Moderator");
          userManager.AddToRole(userManager.FindByName("FredMod").Id, "User");
        }

      }
    }
}

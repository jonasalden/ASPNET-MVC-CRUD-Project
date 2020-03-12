using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Projektarbete_MVC1_JonasAlden.Data.Models;

namespace Projektarbete_MVC1_JonasAlden.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Data\Migrations";
        }

        protected override void Seed(DataContext context)
        {
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var role = new IdentityRole { Name = "Admin" };
                roleManager.Create(role);
            }
            if (!context.Users.Any(u => u.UserName == "Admin"))
            {
                var userStore = new UserStore<User>(context);
                var userManager = new UserManager<User>(userStore);
                var user = new User
                {
                    UserName = "Admin",
                    Firstname = "Jonas",
                    Surname = "Aldén",
                    PhoneNumber = "070-112233",
                    Email = "jonas.alden@spelforetaget.com",
                    Address = "Dyltaroad 6",
                    Experience = "12y",
                    EmployedDate = DateTime.Today,
                    Domain = "Teamleader",
                };
                userManager.Create(user, "test123");
                userManager.AddToRole(user.Id, "Admin");
            }

            if (!context.Roles.Any(r => r.Name == "Projectleader"))
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var role = new IdentityRole { Name = "Projectleader" };
                roleManager.Create(role);
            }
            if (!context.Users.Any(u => u.UserName == "Pert"))
            {
                var userStore = new UserStore<User>(context);
                var userManager = new UserManager<User>(userStore);
                var user = new User
                {
                    UserName = "Pert",
                    Firstname = "Pert",
                    Surname = "Tottolainen",
                    PhoneNumber = "019-242424",
                    Email = "pert.tottolainen@spelforetaget.com",
                    Address = "Dyltagate 32",
                    Experience = "3y",
                    EmployedDate = DateTime.Today,
                    Domain = "3D Animator"

                };
                userManager.Create(user, "test123");
                userManager.AddToRole(user.Id, "Projectleader");
            }
            if (!context.Roles.Any(r => r.Name == "Employee"))
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var role = new IdentityRole { Name = "Employee" };
                roleManager.Create(role);
            }
            if (!context.Users.Any(u => u.UserName == "Joel"))
            {
                var userStore = new UserStore<User>(context);
                var userManager = new UserManager<User>(userStore);
                var user = new User
                {
                    UserName = "Joel",
                    Firstname = "Joel",
                    Surname = "Gustavsson",
                    PhoneNumber = "019-203913",
                    Email = "joel.gustavsson@spelforetaget.com",
                    Address = "Körgatan 60A",
                    Experience = "5y",
                    EmployedDate = DateTime.Today,
                    Domain = "Programmer"

                };
                userManager.Create(user, "test123");
                userManager.AddToRole(user.Id, "Employee");
            }
        }
    }
}

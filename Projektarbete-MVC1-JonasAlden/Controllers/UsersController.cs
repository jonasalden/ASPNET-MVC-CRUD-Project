using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Projektarbete_MVC1_JonasAlden.Data;
using Projektarbete_MVC1_JonasAlden.Data.Models;
using Projektarbete_MVC1_JonasAlden.ViewModels.Projects;
using Projektarbete_MVC1_JonasAlden.ViewModels.Users;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Projektarbete_MVC1_JonasAlden.Controllers
{
    public class UsersController : Controller
    {
        #region Fields
        private ApplicationUserManager _userManager;
        #endregion

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        #region Methods
        [Authorize]
        public ActionResult Index()
        {
            using (var db = new DataContext())
            {
                var model = new UsersViewModel()
                {
                    Users = db.Users.SortBy("Firstname").ToList()
                };
                return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Details(string id)
        {
            using (var db = new DataContext())
            {
                var user = db.Users.Include(x => x.Projects.Select(p => p.Users)).FirstOrDefault(x => x.Id == id);
                if (user != null)
                {
                    var store = new UserStore<User>(db);
                    var manager = new ApplicationUserManager(store);

                    var employee = new UserDetailsViewModel()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        FirstName = user.Firstname,
                        SurName = user.Surname,
                        Email = user.Email,
                        Address = user.Address,
                        EmployDate = user.EmployedDate,
                        PhoneNumber = user.PhoneNumber,
                        Experience = user.Experience,
                        Domain = user.Domain,
                        Admin = manager.IsInRole(user.Id, "Admin"),
                        Projectleader = manager.IsInRole(user.Id, "Projectleader"),

                        Projects = user.Projects.Select(p => new ProjectListViewModel
                        {
                            Id = p.Id,
                            Title = p.Title,
                            Platform = p.Platform,
                            ProjectLeader = p.ProjectleaderName,
                            ReleaseDate = p.ReleaseDate,
                            Employees = p.Users.Count
                        }).ToList()
                    };
                    return View(employee);
                }
                return HttpNotFound();
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Details(UserDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(model.Id);

                user.Firstname = model.FirstName;
                user.Surname = model.SurName;
                user.Email = model.Email;
                user.Address = model.Address;
                user.Domain = model.Domain;
                user.EmployedDate = model.EmployDate;
                user.Experience = model.Experience;
                user.PhoneNumber = model.PhoneNumber;
                UserManager.Update(user);
                if (model.Admin)
                {
                    UserManager.AddToRole(model.Id, "Admin");
                }
                else
                {
                    UserManager.RemoveFromRole(model.Id, "Admin");
                }
                if (model.Projectleader)
                {
                    UserManager.AddToRole(model.Id, "Projectleader");
                }
                else
                {
                    UserManager.RemoveFromRole(model.Id, "Projectleader");
                }

                TempData["Message"] = user.Firstname + " was successfully Updated!";
                TempData["Type"] = "alert-success";

                return RedirectToAction("Details/" + user.Id);
            }
            else
            {
                TempData["Message"] = "Something went wrong!";
                TempData["Type"] = "alert-danger";
                var user = UserManager.FindById(model.Id);
                return RedirectToAction("Details/" + user.Id);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            using (var db = new DataContext())
            {
                if (id != null)
                {
                    var store = new UserStore<User>(db);
                    var manager = new ApplicationUserManager(store);
                    var user = manager.FindById(id);
                    manager.Delete(user);
                    db.SaveChanges();
                    TempData["Message"] = "The user " + user.Firstname + " was removed";
                    TempData["Type"] = "alert-danger";
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Add()
        {
            var GetRoles = new CreateUserViewModel
            {
                Rolelist = (new DataContext()).Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                    new SelectListItem
                    {
                        Value = rr.Name.ToString(),
                        Text = rr.Name
                    }).ToList()
            };
            return View(GetRoles);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Add(CreateUserViewModel newUser)
        {
            using (var db = new DataContext())
            {
                if (ModelState.IsValid)
                {
                    var store = new UserStore<User>(db);
                    var manager = new ApplicationUserManager(store);
                    var user = new User
                    {
                        UserName = newUser.UserName,
                        Firstname = newUser.Firstname,
                        Surname = newUser.Surname,
                        Email = newUser.Email,
                        Domain = newUser.Domain,
                        PasswordHash = newUser.PasswordHash,
                        PhoneNumber = newUser.PhoneNumber,
                        Experience = newUser.Experience,
                        EmployedDate = newUser.EmployedDate,
                        Address = newUser.Address
                    };
                    db.Users.Add(user);
                    manager.Create(user, user.PasswordHash);
                    manager.AddToRole(user.Id, newUser.SelectedRole);
                    db.SaveChanges();
                    TempData["Message"] = user.Firstname + " was successfully added!";
                    TempData["Type"] = "alert-success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "Something went wrong!";
                    TempData["Type"] = "alert-danger";
                    return RedirectToAction("Add");
                }
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel password)
        {
            if (password.ConfirmPassword != null && password.NewPassword != null && password.OldPassword != null)
            {
                using (var db = new DataContext())
                {
                    var store = new UserStore<User>(db);
                    var manager = new ApplicationUserManager(store);
                    var user = await manager.FindAsync(User.Identity.Name, password.OldPassword);

                    if (!manager.CheckPassword(user, password.OldPassword))
                    {
                        TempData["Message"] = "Incorrect creedentials";
                        TempData["Type"] = "alert-danger";
                        return View("ChangePassword");
                    }
                    else
                    {
                        if (password.NewPassword != password.ConfirmPassword)
                        {
                            TempData["Message"] = "Match the new passwords!";
                            TempData["Type"] = "alert-danger";
                            return View("ChangePassword");
                        }
                        else
                        {

                            var hashedNewPassword = manager.PasswordHasher.HashPassword(password.NewPassword);
                            await store.SetPasswordHashAsync(user, hashedNewPassword);
                            await store.UpdateAsync(user);
                            TempData["Message"] = "Password was successfully updated!";
                            TempData["Type"] = "alert-success";
                        }

                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                TempData["Message"] = "You need to fill the fields!";
                TempData["Type"] = "alert-danger";
                return View("ChangePassword");
            }
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
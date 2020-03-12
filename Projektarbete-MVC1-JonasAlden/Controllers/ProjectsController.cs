using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Projektarbete_MVC1_JonasAlden.Data;
using Projektarbete_MVC1_JonasAlden.Data.Models;
using Projektarbete_MVC1_JonasAlden.ViewModels.Users;
using Projektarbete_MVC1_JonasAlden.ViewModels.Projects;

namespace Projektarbete_MVC1_JonasAlden.Controllers
{
    public class ProjectsController : Controller
    {
        #region Methods
        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                using (var db = new DataContext())
                {
                    var model = new ProjectsViewModel()
                    {
                        Projects = db.Projects.Select(p => new ProjectListViewModel
                        {
                            Id = p.Id,
                            Title = p.Title,
                            Platform = p.Platform,
                            ProjectLeader = p.ProjectleaderName,
                            ReleaseDate = p.ReleaseDate,
                            Employees = p.Users.Count
                        }).ToList()
                    };
                    return View(model);
                }
            }
            else if (User.IsInRole("Projectleader"))
            {
                return MyProjects();
            }
            else if (User.IsInRole("Employee"))
            {
                return EmpProjects();
            }
            return View();
        }

        [Authorize(Roles = "Projectleader")]
        public ActionResult MyProjects()
        {
            using (var db = new DataContext())
            {
                var userId = User.Identity.GetUserId();
                var model = new ProjectsViewModel()
                {
                    Projects =
                        db.Projects.Include(x => x.Users.Select(p => p.Projects))
                            .Where(x => x.ProjectleaderId == userId)
                            .Select(p => new ProjectListViewModel
                            {
                                Id = p.Id,
                                Title = p.Title,
                                Platform = p.Platform,
                                ProjectLeader = p.ProjectleaderName,
                                ReleaseDate = p.ReleaseDate,
                                Employees = p.Users.Count
                            }).ToList()
                };
                return View("Index", model);
            };
        }

        [Authorize(Roles = "Employee")]
        public ActionResult EmpProjects()
        {
            using (var db = new DataContext())
            {
                var userId = User.Identity.GetUserId();
                var model = new ProjectsViewModel()
                {
                    Projects =
                        db.Projects.Include(x => x.Users.Select(p => p.Projects)).Where(n => n.Users.Any(y => y.Id == userId))
                            .Select(p => new ProjectListViewModel
                            {
                                Id = p.Id,
                                Title = p.Title,
                                Platform = p.Platform,
                                ProjectLeader = p.ProjectleaderName,
                                ReleaseDate = p.ReleaseDate,
                                Employees = p.Users.Count
                            }).ToList()
                };
                return View("Index", model);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Add(CreateProjectViewModel newProject)
        {
            using (var db = new DataContext())
            {
                var user = db.Users.FirstOrDefault(x => x.Firstname == newProject.FirstName && x.Surname == newProject.SurName);
                var store = new UserStore<User>(db);
                var manager = new ApplicationUserManager(store);
                if (user == null)
                {
                    TempData["Message"] = "User does not exist.";
                    TempData["Type"] = "alert-danger";
                    return RedirectToAction("Add");
                }
                if (ModelState.IsValid && manager.IsInRole(user.Id, "Projectleader"))
                {
                    var project = new Project()
                    {
                        Id = newProject.Id,
                        Title = newProject.Title,
                        ProjectleaderId = user.Id,
                        ProjectleaderName = newProject.FirstName,
                        Platform = newProject.Platform,
                        ReleaseDate = newProject.ReleaseDate,
                        CreateDate = DateTime.Today,
                        Description = newProject.Description,
                    };
                    db.Projects.Add(project);
                    db.SaveChanges();
                    TempData["Message"] = project.Title + " was successfully added!";
                    TempData["Type"] = "alert-success";
                }
                else
                {
                    TempData["Message"] = "Something went wrong. Are you sure he is projectleader?";
                    TempData["Type"] = "alert-danger";
                    return RedirectToAction("Add");
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            using (var db = new DataContext())
            {
                if (db.Projects.Any(x => x.Id == id))
                {
                    var getId = db.Projects.First(x => x.Id == id);
                    db.Projects.Remove(getId);
                    db.SaveChanges();
                    TempData["Message"] = getId.Title + " was successfully Removed!";
                    TempData["Type"] = "alert-success";
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Projectleader")]
        public ActionResult DeleteUser(string userid, int projectid)
        {
            using (var db = new DataContext())
            {
                var user = db.Users.FirstOrDefault(x => x.Id == userid);
                var project = db.Projects.Where(x => x.Id == projectid).Include(y => y.Users).SingleOrDefault();

                project.Users.Remove(user);

                db.SaveChanges();
                TempData["Message"] = user.Firstname + " was successfully Removed!";
                TempData["Type"] = "alert-danger";
                return RedirectToAction("Details/" + projectid);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Projectleader")]
        public ActionResult AddUser(int projectid)
        {
            using (var db = new DataContext())
            {
                var model = new AddUserProjectViewModel()
                {
                    ProjectId = projectid,
                    Users = db.Users.SortBy("Firstname").ToList()
                };
                return View(model);
            }
        }

        [Authorize(Roles = "Admin, Projectleader")]
        public ActionResult AddUsers(string userid, int projectid)
        {
            using (var db = new DataContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userid);
                var project = db.Projects.Where(p => p.Id == projectid).Include(u => u.Users).SingleOrDefault();
                project.Users.Add(user);
                db.SaveChanges();
                TempData["Message"] = user.Firstname + " was successfully Added!";
                TempData["Type"] = "alert-success";
                return RedirectToAction("Details/" + project.Id);
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            using (var db = new DataContext())
            {
                var projectQuery = db.Projects.Include(x => x.Users.Select(p => p.Projects)).FirstOrDefault(x => x.Id == id);
                if (projectQuery != null)
                {
                    var project = new ProjectDetailsViewModel()
                    {
                        Id = projectQuery.Id,
                        Title = projectQuery.Title,
                        Platform = projectQuery.Platform,
                        CreatedDate = projectQuery.CreateDate,
                        ReleaseDate = projectQuery.ReleaseDate,
                        Description = projectQuery.Description,
                        ProjectleaderId = projectQuery.ProjectleaderId,
                        Projectleader = projectQuery.ProjectleaderName,
                        
                        Users = projectQuery.Users.Select(u => new ProjectUserListViewModel()
                        {
                            Id = u.Id,
                            FirstName = u.Firstname,
                            SurName = u.Surname,
                            Email = u.Email,
                            Domain = u.Domain,
                            PhoneNumber = u.PhoneNumber
                        }).ToList()
                    };
                    return View(project);
                }
                else
                {
                return HttpNotFound();
                }
            }
        }

        [Authorize(Roles = "Admin, Projectleader")]
        [HttpPost]
        public ActionResult Details(ProjectDetailsViewModel updatedProject)
        {
            using (var db = new DataContext())
            {
                if ((User.Identity.GetUserId() == updatedProject.ProjectleaderId) || (User.IsInRole("Admin")))
                {
                    var project = db.Projects.FirstOrDefault(x => x.Id == updatedProject.Id);
                    if (ModelState.IsValid)
                    {
                        project.Title = updatedProject.Title;
                        project.Platform = updatedProject.Platform;
                        project.CreateDate = updatedProject.CreatedDate;
                        project.Description = updatedProject.Description;
                        project.ReleaseDate = updatedProject.ReleaseDate;
                        project.ProjectleaderId = updatedProject.ProjectleaderId;
                        project.ProjectleaderName = updatedProject.Projectleader;

                        TempData["Message"] = updatedProject.Title + " was successfully Updated!";
                        TempData["Type"] = "alert-success";
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Message"] = "Something went wrong";
                        TempData["Type"] = "alert-danger";
                        return RedirectToAction("Details/" + project.Id);
                    }
                }
                else
                {
                    TempData["Message"] = "You are not authorized to edit this project.";
                    TempData["Type"] = "alert-danger";
                    return RedirectToAction("Index");
                }
            }
        }
        #endregion
    }
}


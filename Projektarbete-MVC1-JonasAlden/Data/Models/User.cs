using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Projektarbete_MVC1_JonasAlden.Data.Models
{
    public class User : IdentityUser
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Experience { get; set; }
        public DateTime EmployedDate { get; set; }
        public string Address { get; set; }
        public string Domain { get; set; }
        public ICollection<Project> Projects { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}
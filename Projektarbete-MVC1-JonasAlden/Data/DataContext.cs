using Microsoft.AspNet.Identity.EntityFramework;
using Projektarbete_MVC1_JonasAlden.Data.Models;
using System.Data.Entity;

namespace Projektarbete_MVC1_JonasAlden.Data
{

    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Project> Projects { get; set; }
        public DataContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static DataContext Create()
        {
            return new DataContext();
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>().ToTable("Users");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        }
    }
}
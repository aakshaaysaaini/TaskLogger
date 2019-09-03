using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskLoggerApplication.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public bool IsAdmin  { get; set; }
        public virtual Project Project { get; set; }
        public virtual List<Tasks> Tasks { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new LogTaskDBInitializer<ApplicationDbContext>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<TaskLoggerApplication.Models.Project> Projects { get; set; }

        public System.Data.Entity.DbSet<TaskLoggerApplication.Models.Tasks> Tasks { get; set; }
    }

    public class LogTaskDBInitializer<TContext> : DropCreateDatabaseIfModelChanges<TContext> where TContext : DbContext
    {
        protected override void Seed(TContext context)
        {
            var _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var _list = new[] {
            new {Email = "pragatisachdevs@qainfotech.com", Username = "pragatisachdeva@qainfotech.com", FullName = "Pragati Sachdeva"},
        };

            foreach (var item in _list)
            {
                var _user = new ApplicationUser()
                {
                    Email = item.Email,
                    UserName = item.Username,
                    IsAdmin = true
                };
                _userManager.Create(_user, "Password@1");
            }
            base.Seed(context);
        }
    }
}
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Commu_balance.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
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
        }
        public DbSet<Beneficiary_Signature> Beneficiary_Signatures { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Tracking> Trackings { get; set; }
        public DbSet<User_Category> User_Categories { get; set; }


        public DbSet<IdentityUserRole> UserInRole { get; set; }
        // public DbSet<ApplicationUser> appUsers { get; set; }
        public DbSet<ApplicationRole> appRoles { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

		public System.Data.Entity.DbSet<Commu_balance.Models.RequestStatus> RequestStatus { get; set; }

		public System.Data.Entity.DbSet<Commu_balance.Models.QRCode> QRCodes { get; set; }
	}
}
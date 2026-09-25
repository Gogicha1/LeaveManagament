using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace LeaveManagament.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "cbd2865a-8774-4514-927d-125b9b71ba84",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE",
                    ConcurrencyStamp = "7d55f371-4acd-4d63-a1fd-420489b3be2d"
                },
                new IdentityRole
                {
                    Id = "4d6877fe-c183-4d55-80d6-2e53cfc87273",
                    Name = "Supervisor",
                    NormalizedName = "SUPERVISOR",
                    ConcurrencyStamp = "64e3d12e-c8a1-486f-8c33-9384e55d2951"
                },
                new IdentityRole
                {
                    Id = "ee3106d3-f7fe-4a40-aa30-73bdbf8730f8",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    ConcurrencyStamp = "89d38a6a-f9ec-48a7-8267-b1bf12111b54"
                });

            builder.Entity<ApplicationUser>()
                .HasData(new ApplicationUser
                {
                    Id = "0bf3f41f-9b52-4233-918c-7e9d16177850",
                    UserName = "admin@localhost.com",
                    NormalizedUserName = "ADMIN@LOCALHOST.COM",
                    Email = "admin@localhost.com",
                    NormalizedEmail = "ADMIN@LOCALHOST.COM",
                    PasswordHash = "AQAAAAIAAYagAAAAEA9fL7HUHhLKG7f1iGrs5Y7dUjEUYbRzwxTAfFNCG5Vi85n5OI5yXHJQJKnQAG8Rkg==",
                    EmailConfirmed = true,
                    SecurityStamp = "15b3fb44-a359-4ea3-9c82-2e4f6e7edf3a",
                    ConcurrencyStamp = "d0242dd8-335d-4363-8cea-f40600065882",
                    FirstName = "Default",
                    LastName = "Admin",
                    DateOfBirth = new DateOnly(1990, 1, 1)

                });
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "ee3106d3-f7fe-4a40-aa30-73bdbf8730f8",
                    UserId = "0bf3f41f-9b52-4233-918c-7e9d16177850"
                }
            );

            builder.Entity<LeaveType>().HasData(
                new LeaveType
                {
                    Id = 1,
                    Name = "Vacation",
                    NumberOfDays = 24,
                    DateCreated = new DateTime(2026, 1, 1),
                    DateModified = new DateTime(2026, 1, 1)
                },
                new LeaveType
                {
                    Id = 2,
                    Name = "Sick Leave",
                    NumberOfDays = 10,
                    DateCreated = new DateTime(2026, 1, 1),
                    DateModified = new DateTime(2026, 1, 1)
                });
        }

        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
    }
}

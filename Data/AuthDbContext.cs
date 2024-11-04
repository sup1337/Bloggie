using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Bloggie.web.Data;

public class AuthDbContext : IdentityDbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        //seeding roles USER ADMIN SUPERADMIN


        var adminRoleId = "f07f938d-6ad3-4ed9-1179-08da6959dddd";
        var superAdminRoleId = "d4e5f678-90ab-cdef-1234-567890abcd12";
        var userRoleId = "b2c3d4e5-f678-90ab-cdef-1234567890ab";

        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "Admin",
                Id = adminRoleId,
                ConcurrencyStamp = adminRoleId
            },
            new IdentityRole
            {
                Name = "SuperAdmin",
                NormalizedName = "SuperAdmin",
                Id = superAdminRoleId,
                ConcurrencyStamp = superAdminRoleId
            },
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "User",
                Id = userRoleId,
                ConcurrencyStamp = userRoleId
            }
        };

        builder.Entity<IdentityRole>().HasData(roles);
        //seeding SUPERADMINUSER
        var superAdminId = "e5f67890-abcd-ef12-3456-7890abcdef12";
        var superAdminUser = new IdentityUser
        {
            UserName = "superadmin@bloggie.com",
            Email = "superadmin@bloggiee.com",
            NormalizedEmail = "superadmin@bloggiee.com".ToUpper(),
            NormalizedUserName = "superadmin@bloggiee.com".ToUpper(),
            Id = superAdminId
        };
        superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
            .HashPassword(superAdminUser, "12345");

        builder.Entity<IdentityUser>().HasData(superAdminUser);


        //add all roles to superadmin
        var superAdminRoles = new List<IdentityUserRole<string>>
        {
            new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = superAdminId
            },
            new IdentityUserRole<string>
            {
                RoleId = superAdminRoleId,
                UserId = superAdminId
            },
            new IdentityUserRole<string>
            {
                RoleId = userRoleId,
                UserId = superAdminId
            }
        };
        builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
    }
}
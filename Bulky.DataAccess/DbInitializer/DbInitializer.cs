using Bulky.DataAccess.Data;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bulky.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(AppDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<DbInitializer> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public void RunMigrations()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, ex);
                throw;
            }
        }

        public void SeedRoles(string adminUserPassword)
        {
            try
            {
                if (!_roleManager.RoleExistsAsync(CONST_Roles.Customer).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(CONST_Roles.Customer)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(CONST_Roles.Company)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(CONST_Roles.Admin)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(CONST_Roles.Employee)).GetAwaiter().GetResult();

                    var result = _userManager.CreateAsync(user: new ApplicationUser
                    {
                        UserName = "admin@email.com",
                        Email = "admin@email.com",
                        Name = "Admin",
                        PhoneNumber = "1112223333",
                        StreetAddress = "test 123 Ave",
                        State = "IL",
                        PostalCode = "23422",
                        City = "Chicago",
                        LockoutEnabled = false,
                    }, password: adminUserPassword)
                        .GetAwaiter().GetResult();
                    if (!result.Succeeded) throw new Exception("Create userAdmin failed");

                    var user = _context.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@email.com") ?? throw new Exception("Get userAdmin failed");
                    _userManager.AddToRoleAsync(user, CONST_Roles.Admin).GetAwaiter().GetResult();
                }
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, ex);
                throw;
            }
        }
    }
}

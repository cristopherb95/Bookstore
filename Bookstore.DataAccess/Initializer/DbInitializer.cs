using System;
using System.Linq;
using Bookstore.DataAccess.Data;
using Bookstore.Models;
using Bookstore.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.DataAccess.Initializer
{
    public class DbInitializer: IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
                
                if (_db.Roles.Any(r => r.Name == SD.Role_Admin)) 
                    return;
            
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Company)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Individual)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@mail.com",
                    Email = "admin@mail.com",
                    EmailConfirmed = true,
                    Name = "Bookstore Admin"
                }, "Admin123!").GetAwaiter().GetResult();
            
                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@mail.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {

            }
        }
        
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TodoMvc.Models;
using TodoMvc.Models.View;
using System.Linq;
using System.Collections.Generic;

namespace TodoMvc.Controllers {
    [Authorize(Roles = "Administrator")]
    public class ManageUsersController : Controller {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageUsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index() {
            var admins = await _userManager.GetUsersInRoleAsync(Constants.AdministratorRole);

            // var everyone = await _userManager.Users.ToArrayAsync();
            var users = await _userManager.GetUsersInRoleAsync(Constants.UserRole);

            var model = new ManageUsersViewModel{
                Administrators = admins,
                Users = users
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(string Id) {
            var user = await _userManager.Users
                .Where(x => x.Id == Id)
                .SingleOrDefaultAsync();
                
            if (user == null)
                return BadRequest();

            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            IdentityRole[] roles = await _roleManager.Roles.ToArrayAsync();

            List<IdentityRole> tempList = new List<IdentityRole>();
            for (int i = 0; i < roles.Length; i++)
            {
                if(!userRoles.Contains(roles[i].Name))
                    tempList.Add(roles[i]);
            }

            roles = tempList.ToArray();

            var model = new ManageUsersEditViewModel{
                UserId = Id,
                UserRoles = userRoles,
                Roles = roles
            };

            return View(model);
        }

        public async Task<IActionResult> MakeAdministrator(string Id) {
            var user = await _userManager.Users
                .Where(x => x.Id == Id)
                .SingleOrDefaultAsync();
                
            if (user == null)
                return BadRequest();

            await _userManager.RemoveFromRoleAsync(user, Constants.UserRole);
            await _userManager.AddToRoleAsync(user, Constants.AdministratorRole);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UnmakeAdministrator(string Id) {
            var user = await _userManager.Users
                .Where(x => x.Id == Id)
                .SingleOrDefaultAsync();
                
            if (user == null)
                return BadRequest();

            await _userManager.RemoveFromRoleAsync(user, Constants.AdministratorRole);
            await _userManager.AddToRoleAsync(user, Constants.UserRole);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AdicionarRole(string Id, string Role) {
            var user = await _userManager.Users
                .Where(x => x.Id == Id)
                .SingleOrDefaultAsync();
                
            if (user == null)
                return BadRequest();

            await _userManager.AddToRoleAsync(user, Role);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveRole(string Id, string Role) {
            var user = await _userManager.Users
                .Where(x => x.Id == Id)
                .SingleOrDefaultAsync();
                
            if (user == null)
                return BadRequest();

            await _userManager.RemoveFromRoleAsync(user, Role);

            return RedirectToAction(nameof(Index));
        }
    }
}
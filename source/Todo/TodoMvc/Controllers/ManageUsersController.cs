using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TodoMvc.Models;
using TodoMvc.Models.View;
using System.Linq;

namespace TodoMvc.Controllers {
    [Authorize(Roles = "Administrator")]
    public class ManageUsersController : Controller {
        private readonly UserManager<ApplicationUser> _userManager;
        public ManageUsersController(UserManager<ApplicationUser> userManager) {
            _userManager = userManager;
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
    }
}
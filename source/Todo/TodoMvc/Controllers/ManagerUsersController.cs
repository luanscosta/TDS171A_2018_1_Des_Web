using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TodoMvc.Models;
using TodoMvc.Models.View;

namespace TodoMvc.Controllers {
    [Authorize(Roles = "Administrator")]
    public class ManagerUsersController : Controller {
        private readonly UserManager<ApplicationUser> _userManager;
        public ManagerUsersController(UserManager<ApplicationUser> userManager) {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() {
            var admins = await _userManager.GetUsersInRoleAsync("Administrator");

            var everyone = await _userManager.Users.ToArrayAsync();

            var model = new ManagerUsersViewModel{
                Administrators = admins,
                Everyone = everyone
            };

            return View(model);
        }
    }
}
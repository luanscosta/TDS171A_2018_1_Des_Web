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
    public class ManageRolesController : Controller {

        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageRolesController(RoleManager<IdentityRole> roleManager) {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index() {
            var roles = await _roleManager.Roles.ToArrayAsync();

            var model = new ManageRolesViewModel{
                Roles = roles
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole role) {
            if(ModelState.IsValid) {
                await _roleManager.CreateAsync(role);
                return RedirectToAction(nameof(Index));
            }

            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string Id) {
            if(Id == null) {
                return NotFound();
            }

            var role = await _roleManager.Roles
                .Where(x => x.Id == Id)
                .SingleOrDefaultAsync();
                
            if (role == null)
                return BadRequest();

            await _roleManager.DeleteAsync(role);
            
            return RedirectToAction(nameof(Index));
        }
    }
}
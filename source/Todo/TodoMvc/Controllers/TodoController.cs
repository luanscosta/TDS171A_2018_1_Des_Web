using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoMvc.Services;
using TodoMvc.Models.View;
using TodoMvc.Models;

namespace TodoMvc.Controllers {
    [Authorize]
    public class TodoController : Controller {
        private readonly ITodoItemService _todoItemService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoController(ITodoItemService todoItemsService, UserManager<ApplicationUser> userManager) {
            _todoItemService = todoItemsService;
            _userManager = userManager;
        }

        // Action GET
        public async Task<IActionResult> Index() {
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null) {
                return Challenge();
            }

            var todoItems = await _todoItemService.GetIncompleteItemsAsync(currentUser);
            
            var viewModel = new TodoViewModel{
                Items = todoItems
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UserList(string Id) {
            var user = await _userManager.Users
                .Where(x => x.Id == Id)
                .SingleOrDefaultAsync();
                
            if (user == null)
                return BadRequest();

            var todoItems = await _todoItemService.GetIncompleteItemsAsync(user);
            
            var viewModel = new TodoViewModel{
                Items = todoItems,
                User = user
            };

            return View(viewModel);
        }

        // Action POST
        public async Task<ActionResult> AddItem(NewTodoItem newTodoItem) {
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null) {
                return Unauthorized();
            }

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var successful = await _todoItemService.AddItemAsync(newTodoItem, currentUser);

            if(!successful) {
                return BadRequest(new { Error= "Could not add item"});
            }

            return Ok(successful);
        }

        public async Task<IActionResult> MarkDone(Guid id) {
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null) {
                return Unauthorized();
            }
            if (id == Guid.Empty)
                return BadRequest();

            var successful = await _todoItemService.MarkDoneAsync(id, currentUser);

            return Ok(successful);
        }
    }
}
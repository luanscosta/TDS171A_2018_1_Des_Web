using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoMvc.Services;
using TodoMvc.Models.View;
using TodoMvc.Models;

namespace TodoMvc.Controllers {
    public class TodoController : Controller {
        private readonly ITodoItemService _todoItemService;

        public TodoController(ITodoItemService todoItemsService) {
            _todoItemService = todoItemsService;
        }

        // Action GET
        public async Task<IActionResult> Index() {
            var todoItems = await _todoItemService.GetIncompleteItemsAsync();
            
            var viewModel = new TodoViewModel{
                Items = todoItems
            };

            return View(viewModel);
        }

        // Action POST
        public async Task<ActionResult> AddItem(NewTodoItem newTodoItem) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var successful = await _todoItemService.AddItemAsync(newTodoItem);

            if(!successful) {
                return BadRequest(new { Error= "Could not add item"});
            }

            return Ok();
        }
    }
}
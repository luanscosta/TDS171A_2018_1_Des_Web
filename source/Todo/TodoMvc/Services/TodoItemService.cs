using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoMvc.Models;
using TodoMvc.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TodoMvc.Services {
    public class TodoItemService : ITodoItemService {
        private readonly ApplicationDbContext _context;

        public TodoItemService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TodoItem>> GetIncompleteItemsAsync() {
            var items = await _context
                .Items
                .Where(x => x.IsDone == false)
                .ToArrayAsync();
            return items;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoMvc.Models;

namespace TodoMvc.Services {
    public interface ITodoItemService {
        Task<IEnumerable<TodoItem>> GetIncompleteItemsAsync(ApplicationUser currentUser);
        Task<bool> AddItemAsync(NewTodoItem newTodoItem, ApplicationUser currentUser);
        Task<bool> MarkDoneAsync(Guid id, ApplicationUser currentUser);
    }
}
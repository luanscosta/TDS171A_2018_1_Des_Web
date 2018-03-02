using System.Collections.Generic;

namespace TodoMvc.Models.View {
    public class TodoViewModel {
        public IEnumerable<TodoItem> Items {get; set;}
    }
}
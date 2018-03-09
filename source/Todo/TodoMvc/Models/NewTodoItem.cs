using System;
using System.ComponentModel.DataAnnotations;

namespace TodoMvc.Models {
    public class NewTodoItem {
        [Required]
        public string Title {get; set;}
    }
}
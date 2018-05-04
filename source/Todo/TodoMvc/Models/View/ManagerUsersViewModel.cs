using System.Collections.Generic;
using TodoMvc.Models;

namespace TodoMvc.Models.View {
    public class ManagerUsersViewModel {
        public IEnumerable<ApplicationUser> Administrators {get; set; }
        public IEnumerable<ApplicationUser> Users {get; set; }
    }
}
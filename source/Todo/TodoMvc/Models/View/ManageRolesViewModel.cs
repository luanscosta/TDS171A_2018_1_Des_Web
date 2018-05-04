using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using TodoMvc.Models;

namespace TodoMvc.Models.View {
    public class ManageRolesViewModel {
        public IEnumerable<IdentityRole> Roles { get; set; }
    }
}
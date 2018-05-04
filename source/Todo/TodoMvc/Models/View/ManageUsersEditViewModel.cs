using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TodoMvc.Models.View {
    public class ManageUsersEditViewModel {
        public string UserId { get; set; }
        public IList<string> UserRoles { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }
        public IdentityRole Role { get; set; }
    }
}
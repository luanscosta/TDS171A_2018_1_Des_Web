using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoMvc.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Required]
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}

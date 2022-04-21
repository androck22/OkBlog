using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Contracts.Models.Account
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } // UserName

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}

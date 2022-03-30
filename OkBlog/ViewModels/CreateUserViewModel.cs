using System.ComponentModel.DataAnnotations;

namespace OkBlog.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; } = string.Empty;
    }
}

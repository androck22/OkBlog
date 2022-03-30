using System.ComponentModel.DataAnnotations;

namespace OkBlog.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
    }
}

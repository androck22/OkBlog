using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OkBlog.Models.Db
{
    public class Tag
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
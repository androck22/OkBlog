using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Posts
{
    public class AddPostRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public string Author { get; set; } 
    }
}

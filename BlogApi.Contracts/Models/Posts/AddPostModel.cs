using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApi.Contracts.Models.Posts
{
    public class AddPostModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        [EmailAddress]
        public string Author { get; set; } //UserEmail
        public DateTime Created { get; set; } = DateTime.Now;
    }
}

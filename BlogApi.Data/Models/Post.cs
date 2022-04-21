using BlogApi.Data.Models.Comments;
using System;
using System.Collections.Generic;

namespace BlogApi.Data.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Body { get; set; } = String.Empty;
        public string Author { get; set; } = String.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public string UserId { get; set; } = string.Empty;

        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<MainComment> MainComments { get; set; }
        public ApplicationUser User { get; set; }
    }
}

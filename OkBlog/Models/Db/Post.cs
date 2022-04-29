using OkBlog.Models.Db.Comments;
using OkBlog.ViewModels;
using System;
using System.Collections.Generic;

namespace OkBlog.Models.Db
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Body { get; set; } = String.Empty;
        public string Image { get; set; } = String.Empty;
        public string Author { get; set; } = String.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public string UserId { get; set; } = string.Empty;

        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<MainComment> MainComments { get; set; }
        public ApplicationUser User { get; set; }
    }
}

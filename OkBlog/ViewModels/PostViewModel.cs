using OkBlog.Models.Db;
using OkBlog.Models.Db.Comments;
using System;
using System.Collections.Generic;

namespace OkBlog.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        public string CurrentUserName { get; set; }
        public List<TagViewModel> Tags { get; set; }
        public List<MainComment> MainComments { get; set; } = new List<MainComment>();
    }
}

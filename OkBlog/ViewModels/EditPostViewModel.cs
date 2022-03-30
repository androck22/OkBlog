using OkBlog.Models.Db;
using System.Collections.Generic;

namespace OkBlog.ViewModels
{
    public class EditPostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }

        public List<TagViewModel> Tags { get; set; }
    }
}

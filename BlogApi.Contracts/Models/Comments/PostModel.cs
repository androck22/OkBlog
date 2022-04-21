using BlogApi.Data.Models.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Contracts.Models.Comments
{
    public class PostModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        public string CurrentUserName { get; set; }
        //public List<TagViewModel> Tags { get; set; }
        public List<MainComment> MainComments { get; set; } = new List<MainComment>();
    }
}

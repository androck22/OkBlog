using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Contracts.Models.Posts
{
    public class GetPostsModel
    {
        public int PostAmount { get; set; }
        public List<PostView> Posts { get; set; }
    }

    public class PostView
    {
        public string Title { get; set; }   
        public string Body { get; set; }
        public string Author { get; set; }
        public DateTime Created { get; set; }
    }
}

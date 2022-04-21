using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Contracts.Models.Posts
{
    public class EditPostModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Author { get; set; } //UserEmail
    }
}

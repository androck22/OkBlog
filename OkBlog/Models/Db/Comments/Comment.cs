using System;
using System.Globalization;

namespace OkBlog.Models.Db.Comments 
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public string Author { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
    }
}

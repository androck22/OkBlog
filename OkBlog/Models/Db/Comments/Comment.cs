using System;

namespace OkBlog.Models.Db.Comments 
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public string UserName { get; set; }
    }
}

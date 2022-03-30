using System.Collections.Generic;

namespace OkBlog.Models.Db.Comments
{
    public class MainComment : Comment
    {
        public List<SubComment> SubComments { get; set; }
    }
}

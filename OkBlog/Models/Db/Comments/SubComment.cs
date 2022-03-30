namespace OkBlog.Models.Db.Comments
{
    public class SubComment : Comment
    {
        public int MainCommentId { get; set; }
    }
}

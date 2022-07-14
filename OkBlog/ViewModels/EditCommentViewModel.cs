using System.Globalization;

namespace OkBlog.ViewModels
{
    public class EditCommentViewModel
    {
        public int MainCommentId { get; set; }
        public string Message { get; set; }
        public string Author { get; set; }
        public string CurrentUserName { get; set; } 
        public string UserId { get; set; }
        public int PostId { get; set; }
    }

}

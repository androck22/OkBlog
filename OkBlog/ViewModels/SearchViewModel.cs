using OkBlog.Models.Db;

namespace OkBlog.ViewModels
{
    public class SearchViewModel
    {
        public string FirstName { get; set; } = "name";
        public string LastName { get; set; } = "last name";
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ApplicationUser UserSearch { get; set; }
    }
}

using OkBlog.Models.Db;

namespace OkBlog.ViewModels
{
    public class EditRoleViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

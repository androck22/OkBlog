using BlogApi.Contracts.Models.Posts;
using Swashbuckle.AspNetCore.Filters;

namespace BlogApi.SwaggerExamples.Requests
{
    public class EditPostRequestExample : IExamplesProvider<EditPostModel>
    {
        public EditPostModel GetExamples()
        {
            return new EditPostModel
            {
                Title = "название статьи",
                Body = "содержание статьи",
                Author = "автор статьи"
            };
        }
    }
}

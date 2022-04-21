using BlogApi.Contracts.Models.Posts;
using Swashbuckle.AspNetCore.Filters;

namespace BlogApi.SwaggerExamples.Requests
{
    public class AddPostRequestExample : IExamplesProvider<AddPostModel>
    {
        public AddPostModel GetExamples()
        {
            return new AddPostModel
            {
                Title = "название статьи",
                Body = "содержание статьи",
                Author = "автор статьи"
            };
        }
    }
}

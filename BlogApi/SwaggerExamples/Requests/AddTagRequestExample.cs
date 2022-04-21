using BlogApi.Contracts.Models.Tags;
using Swashbuckle.AspNetCore.Filters;

namespace BlogApi.SwaggerExamples.Requests
{
    public class AddTagRequestExample : IExamplesProvider<AddTagModel>
    {
        public AddTagModel GetExamples()
        {
            return new AddTagModel
            {
                Name = "new tag"
            };
        }
    }
}

using BlogApi.Contracts.Models.Tags;
using Swashbuckle.AspNetCore.Filters;

namespace BlogApi.SwaggerExamples.Requests
{
    public class EditTagRequestExample : IExamplesProvider<EditTagModel>
    {
        public EditTagModel GetExamples()
        {
            return new EditTagModel
            {
                Name = "edit tag"
            };
        }
    }
}

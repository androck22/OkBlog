using BlogApi.Contracts.Models.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace BlogApi.SwaggerExamples.Responses
{
    public class TagResponseExample : IExamplesProvider<TagResponse>
    {
        public TagResponse GetExamples()
        {
            return new TagResponse
            {
                Name = "new tag"
            };
        }
    }
}

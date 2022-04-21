using BlogApi.Contracts.Models.Comments;
using Swashbuckle.AspNetCore.Filters;

namespace BlogApi.SwaggerExamples.Requests
{
    public class CommentRequestExample : IExamplesProvider<AddCommentModel>
    {
        public AddCommentModel GetExamples()
        {
            return new AddCommentModel
            {
                Message = "комментарий",
                PostId = 1
            };
        }
    }
}

using BlogApi.Contracts.Models.Users;
using Swashbuckle.AspNetCore.Filters;

namespace BlogApi.SwaggerExamples.Requests
{
    public class EditUserRequestExample : IExamplesProvider<EditUserModel>
    {
        public EditUserModel GetExamples()
        {
            return new EditUserModel
            {
                Email = "email",
                FirstName = "имя",
                LastName = "фамилия"
            };
        }
    }
}

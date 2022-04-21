using BlogApi.Contracts.Models.Users;
using Swashbuckle.AspNetCore.Filters;

namespace BlogApi.SwaggerExamples.Requests
{
    public class AddUserRequestExample : IExamplesProvider<AddUserModel>
    {
        public AddUserModel GetExamples()
        {
            return new AddUserModel
            {
                Email = "email",
                FirstName = "имя",
                LastName = "фамилия",
                Password = "пароль",
                PasswordConfirm = "подтвердите пароль"
            };
        }
    }
}

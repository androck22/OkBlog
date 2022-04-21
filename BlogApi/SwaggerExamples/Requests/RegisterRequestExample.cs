using BlogApi.Contracts.Models.Account;
using Swashbuckle.AspNetCore.Filters;

namespace BlogApi.SwaggerExamples.Requests
{
    public class RegisterRequestExample : IExamplesProvider<RegisterModel>
    {
        public RegisterModel GetExamples()
        {
            return new RegisterModel
            {
                FirstName = "имя",
                LastName = "фамилия",
                Email = "email",
                Password = "пароль",
                PasswordConfirm = "подтвердите пароль"
            };
        }
    }
}

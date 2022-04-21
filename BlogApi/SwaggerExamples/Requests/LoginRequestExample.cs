using BlogApi.Contracts.Models.Account;
using Swashbuckle.AspNetCore.Filters;

namespace BlogApi.SwaggerExamples.Requests
{
    public class LoginRequestExample : IExamplesProvider<LoginModel>
    {
        public LoginModel GetExamples()
        {
            return new LoginModel
            {
                Email = "email",
                Password = "пароль"
            };
        }
    }
}

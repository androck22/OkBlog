using BlogApi.Contracts.Models.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Contracts.Validators
{
    public class AddUserModelValidator : AbstractValidator<AddUserModel>
    {
        string[] _validRoles;

        public AddUserModelValidator()
        {
            _validRoles = new[]
            {
                "Admin",
                "Moderator",
                "User"
            };

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.PasswordConfirm).NotEmpty();
        }
    }
}

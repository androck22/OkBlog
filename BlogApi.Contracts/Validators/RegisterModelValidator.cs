using BlogApi.Contracts.Models.Account;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Contracts.Validators
{
    public class RegisterModelValidator : AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("Field FirstName cannot be empty");
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Field LastName cannot be empty");
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Field Email cannot be empty")
                .EmailAddress()
                .WithMessage("Fiesl Email must be a valid email address");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Field Password cannot be empty");
            RuleFor(x => x.PasswordConfirm)
                .NotEmpty()
                .WithMessage("Field PasswordConfirm cannot be empty");
        }
    }
}

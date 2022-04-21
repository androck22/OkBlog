using BlogApi.Contracts.Models.Comments;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Contracts.Validators
{
    public class AddCommentModelValidator : AbstractValidator<AddCommentModel>
    {
        public AddCommentModelValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty()
                .WithMessage("Field Message cannot be empty");
            RuleFor(x => x.PostId)
                .NotEmpty()
                .WithMessage("Field PostId cannot be empty");
        }
    }
}

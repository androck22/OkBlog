using BlogApi.Contracts.Models.Posts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Contracts.Validators
{
    public class AddPostModelValidator : AbstractValidator<AddPostModel>
    {
        public AddPostModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Body).NotEmpty();
        }
    }
}

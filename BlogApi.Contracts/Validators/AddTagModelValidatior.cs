using BlogApi.Contracts.Models.Tags;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Contracts.Validators
{
    public class AddTagModelValidatior : AbstractValidator<AddTagModel>
    {
        public AddTagModelValidatior()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
                //.Matches("^[a-zA-Z0-9 ]*$");
        }
    }
}

﻿using System.Threading.Tasks;
using FluentValidation;

namespace Application.Validations
{
    public static class ValidatorExtentions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
                .NotEmpty()
                .MinimumLength(6).WithMessage("Password needs at least 6 characters")
                .Matches("[A-Z]").WithMessage("Password must contain 1 uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain 1 lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain 1 number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain 1 non alphanumeric");
            return options;
        }
    }
}
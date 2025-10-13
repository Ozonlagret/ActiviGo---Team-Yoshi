using Application.DTOs.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public sealed class BookSessionRequestValidator : AbstractValidator<BookSessionRequest>
    {
        public BookSessionRequestValidator()
        {
            RuleFor(x => x.ActivitySessionId)
                .GreaterThan(0).WithMessage("ActivitySessionId måste vara större än 0.");
        }
    }
}

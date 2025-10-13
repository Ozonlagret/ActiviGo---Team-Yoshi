using Application.DTOs.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public sealed class CreateActivitySessionRequestValidator : AbstractValidator<CreateActivitySessionRequest>
    {
        public CreateActivitySessionRequestValidator()
        {
            RuleFor(x => x.ActivityId)
                .GreaterThan(0).WithMessage("ActivityId måste vara större än 0.");

            RuleFor(x => x.LocationId)
                .GreaterThan(0).WithMessage("LocationId måste vara större än 0.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Kapaciteten måste vara större än 0.");

            RuleFor(x => x.StartUtc)
                .NotEmpty().WithMessage("StartUtc måste anges.");

            RuleFor(x => x.EndUtc)
                .NotEmpty().WithMessage("EndUtc måste anges.");

            RuleFor(x => new { x.StartUtc, x.EndUtc })
                .Must(x => x.EndUtc > x.StartUtc)
                .WithMessage("Sluttiden måste vara efter starttiden.");
        }
    }
}

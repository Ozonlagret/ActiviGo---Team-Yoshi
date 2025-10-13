using Application.DTOs.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public sealed class CancelBookingRequestValidator : AbstractValidator<CancelBookingRequest>
    {
        public CancelBookingRequestValidator()
        {
            RuleFor(x => x.BookingId)
                .GreaterThan(0).WithMessage("BookingId måste vara större än 0.");
        }
    }
}

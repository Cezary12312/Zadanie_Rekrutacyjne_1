using FluentValidation;
using Zadanie_Rekrutacyjne_1.Models;

namespace Zadanie_Rekrutacyjne_1.Utilities.Validators
{
    public class DateOfBirthValidator : AbstractValidator<User>
    {
        public DateOfBirthValidator()
        {
            RuleFor(p => p.DateOfBirth)
            .Must(BeAValidAge).WithMessage("Niemożliwa data").NotEmpty();
        }

        protected bool BeAValidAge(DateTime date)
        {
            int currentYear = DateTime.Now.Year;
            int dobYear = date.Year;

            if (dobYear <= currentYear && dobYear > (currentYear - 120))
            {
                return true;
            }

            return false;
        }
    }
}

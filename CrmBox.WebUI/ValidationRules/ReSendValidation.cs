using CrmBox.WebUI.Models;
using FluentValidation;

namespace CrmBox.WebUI.ValidationRules
{
    public class ReSendValidation : AbstractValidator<UpdatePasswordVM>
    {
        public ReSendValidation()
        {


            RuleFor(x => x.Password)
                .NotEmpty()
            .MinimumLength(5).MaximumLength(16);
        }

    }
}

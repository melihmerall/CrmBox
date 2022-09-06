using CrmBox.WebUI.Models;
using FluentValidation;

namespace CrmBox.WebUI.ValidationRules
{
    public class SmsValidation:AbstractValidator<SendSmsVM>
    {
        public SmsValidation()
        {

            RuleFor(x => x.MessageBody).NotNull();

            RuleFor(x => x.PhoneNumber).NotNull();

            RuleFor(x => x.PhoneNumber).MaximumLength(13);
            RuleFor(x => x.MessageBody).MaximumLength(350);



        }
    }
}

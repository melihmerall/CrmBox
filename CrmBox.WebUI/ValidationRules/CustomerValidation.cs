using CrmBox.WebUI.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Twilio.TwiML.Voice;

namespace CrmBox.Application.Services.Customer
{
    public class CustomerValidation : AbstractValidator<AddCustomerVM>
    {
        public CustomerValidation()
        {
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.Email).NotNull();
            RuleFor(x => x.PhoneNumber).NotNull();
            RuleFor(x => x.FirstName).NotNull();
            RuleFor(x => x.LastName).NotNull();
            RuleFor(x => x.CompanyName).NotNull();
            RuleFor(x => x.JobTitle).NotNull();

            RuleFor(x => x.Address).MinimumLength(10);
            RuleFor(x => x.PhoneNumber).MinimumLength(13);
            RuleFor(x => x.Email).MinimumLength(6);
            RuleFor(x => x.FirstName).MinimumLength(3);
            RuleFor(x => x.LastName).MinimumLength(3);
            RuleFor(x => x.CompanyName).MinimumLength(3);
            RuleFor(x => x.JobTitle).MinimumLength(5);


            RuleFor(x => x.Address).MaximumLength(350);
            RuleFor(x => x.PhoneNumber).MaximumLength(14);
            RuleFor(x => x.Email).MaximumLength(50);
            RuleFor(x => x.FirstName).MaximumLength(30);
            RuleFor(x => x.LastName).MaximumLength(30);
            RuleFor(x => x.CompanyName).MaximumLength(20);
            RuleFor(x => x.JobTitle).MaximumLength(15);

         


        }
        private bool IsPhoneNumberMasking(string arg)
        {
            // Proje ilerki safhalarda kullanılabilir. Must komutu içinde çağırılıp.
            Regex regex = new Regex(@"([\(? (\d{0,3})\)?\s]+[-\s])(?=\d{4})");
            return regex.IsMatch(arg);
        }


    }

}

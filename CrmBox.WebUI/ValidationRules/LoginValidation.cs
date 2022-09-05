using CrmBox.Core.Domain.Identity;
using CrmBox.WebUI.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace CrmBox.Application.Services.Customer
{
    public class LoginValidation: AbstractValidator<UserForLoginVM>

    {

        public LoginValidation()
        {
            

            RuleFor(x => x.Username).NotNull();
            RuleFor(x => x.Username).MinimumLength(3);
            RuleFor(x => x.Username).MaximumLength(13);
            RuleFor(x => x.Password)
                    .NotEmpty().MinimumLength(5).MaximumLength(16);
    
        }
        private bool IsPasswordValid(string arg)
        {
            // Proje ilerki safhalarda kullanılabilir. Must komutu içinde çağırılıp.
            Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
            return regex.IsMatch(arg);
        }

    }
}

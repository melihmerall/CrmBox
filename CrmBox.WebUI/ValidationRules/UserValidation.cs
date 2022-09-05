using CrmBox.WebUI.Models;
using FluentValidation;
using FluentValidation.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace CrmBox.Application.Services.Customer
{
    public class UserValidation: AbstractValidator<AddUserVM>
    {
        public UserValidation()
        {
            
            RuleFor(x => x.UserName).NotNull();
            RuleFor(x => x.FirstName).NotNull();
            RuleFor(x => x.LastName).NotNull();
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.Password).NotNull().MinimumLength(5).MaximumLength(16);


            RuleFor(x => x.UserName).MinimumLength(3);
            RuleFor(x => x.FirstName).MinimumLength(3);
            RuleFor(x => x.LastName).MinimumLength(3);


            RuleFor(x => x.UserName).MaximumLength(30);
            RuleFor(x => x.FirstName).MaximumLength(50);
            RuleFor(x => x.LastName).MaximumLength(30);


        }
    }
}

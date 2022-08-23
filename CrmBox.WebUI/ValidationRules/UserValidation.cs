using CrmBox.WebUI.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CrmBox.Application.Services.Customer
{
    public class UserValidation: AbstractValidator<AddUserVM>
    {
        public UserValidation()
        {
            
            RuleFor(x => x.UserName).NotNull().WithMessage("Kullanıcı Adı Boş olamaz.");
            RuleFor(x => x.FirstName).NotNull().WithMessage("İsim Boş olamaz.");
            RuleFor(x => x.LastName).NotNull().WithMessage("Soyisim Boş olamaz.");


            RuleFor(x => x.UserName).MinimumLength(3).WithMessage("En az 3 karakter girin.");
            RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("30 Karakterden fazla olamaz.");
            RuleFor(x => x.LastName).MinimumLength(3).WithMessage("30 karakterden fazla olamaz.");


            RuleFor(x => x.UserName).MaximumLength(30).WithMessage("30 karakterden fazla olamaz..");
            RuleFor(x => x.FirstName).MaximumLength(50).WithMessage("50 Karakterden fazla olamaz.");
            RuleFor(x => x.LastName).MaximumLength(30).WithMessage("30 karakterden fazla olamaz.");

        }
    }
}

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
    public class LoginValidation: AbstractValidator<UserForLoginVM>
    {
        public LoginValidation()
        {

            RuleFor(x => x.Username).NotNull().WithMessage("Kullanıcı Adı Boş olamaz.");
        
           


            RuleFor(x => x.Username).MinimumLength(3).WithMessage("En az 3 karakter girin.");



            RuleFor(x => x.Username).MaximumLength(13).WithMessage("13 karakterden fazla olamaz..");



            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş geçilemez.")
            .MinimumLength(5).WithMessage("En az 5 karakter giriniz.").MaximumLength(16).WithMessage("En fazla 16 karakter girmelisiniz.");



        }
    }
}

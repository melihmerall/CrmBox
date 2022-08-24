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
    public class CustomerValidation: AbstractValidator<Core.Domain.Customer>
    {
        public CustomerValidation()
        {
            RuleFor(x => x.Address).NotEmpty().WithMessage("Adres Boş olamaz.");
            RuleFor(x => x.Email).NotNull().WithMessage("E-Mail Boş olamaz.");
            RuleFor(x => x.PhoneNumber).NotNull().WithMessage("Telefon numarası Boş olamaz.");
            RuleFor(x => x.FirstName).NotNull().WithMessage("İsim Boş olamaz.");
            RuleFor(x => x.LastName).NotNull().WithMessage("Soyisim Boş olamaz.");
            RuleFor(x => x.CompanyName).NotNull().WithMessage("Şirket İsmi Boş olamaz.");
            RuleFor(x => x.JobTitle).NotNull().WithMessage("İş Başlığı Boş olamaz.");

            RuleFor(x => x.Address).MinimumLength(10).WithMessage("En az 10 karakter girin.");
            RuleFor(x => x.PhoneNumber).MinimumLength(11).WithMessage("11 Haneden oluşmalı.");
            RuleFor(x => x.Email).MinimumLength(6).WithMessage("6 Karakterden az olamaz.");
            RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("En az 3 karakter girin.");
            RuleFor(x => x.LastName).MinimumLength(3).WithMessage("En az 3 karakter girin.");
            RuleFor(x => x.CompanyName).MinimumLength(3).WithMessage("En az 3 karakter girin.");
            RuleFor(x => x.JobTitle).MinimumLength(5).WithMessage("En az 5 karakter girin.");


            RuleFor(x => x.Address).MaximumLength(350).WithMessage("350 karakteri geçmeyin!.");
            RuleFor(x => x.PhoneNumber).MaximumLength(11).WithMessage("11 haneden oluşmalı!.");
            RuleFor(x => x.Email).MaximumLength(50).WithMessage("50 Karakterden fazla olamaz.");
            RuleFor(x => x.FirstName).MaximumLength(30).WithMessage("30 karakterden fazla olamaz.");
            RuleFor(x => x.LastName).MaximumLength(30).WithMessage("30 karakterden fazla olamaz.");
            RuleFor(x => x.CompanyName).MaximumLength(20).WithMessage("20 karakteri geçmeyin!.");
            RuleFor(x => x.JobTitle).MaximumLength(15).WithMessage("15 karakteri geçmeyin!.");
        }
    }
}

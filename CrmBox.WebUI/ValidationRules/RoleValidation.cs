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
    public class RoleValidation: AbstractValidator<AddRoleVM>
    {
        public RoleValidation()
        {

            RuleFor(x => x.Name).NotNull();
            
            RuleFor(x => x.Name).MinimumLength(3);

            RuleFor(x => x.Name).MaximumLength(13);


        }
    }
}

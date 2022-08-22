using Autofac;
using CrmBox.Application.Interfaces.Customer;
using CrmBox.Application.Services.Customer;

namespace CrmBox.Infrastructure.Registrations;

public class AutofacRegistration : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CustomerService>().As<ICustomerService>();
        
    }
}
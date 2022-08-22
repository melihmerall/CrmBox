using CrmBox.Core.Domain;

namespace CrmBox.Application.Interfaces.Customer;

public interface ICustomerService : IGenericService<Core.Domain.Customer>
{
    Core.Domain.Customer GetById(int id);

}
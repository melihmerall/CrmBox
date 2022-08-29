using CrmBox.Application.Interfaces.Customer;
using CrmBox.Persistance.Context;

namespace CrmBox.Application.Services.Customer;

public class CustomerService : GenericService<Core.Domain.Customer, CrmBoxContext>, ICustomerService
{
    readonly CrmBoxContext _context;
    public CustomerService(CrmBoxContext context) : base(context)
    {
        _context = context;
    }

    public Core.Domain.Customer GetById(int id)
    {
        Core.Domain.Customer customer = _context.Set<Core.Domain.Customer>().Find(id);
        if (customer != null)
            return customer;
        else
            throw new Exception("Aranılan id ile eşleşen müşteri bulunamadı.");
    }

}

using Microsoft.AspNetCore.Mvc;
using CrmBox.Core.Domain;
using CrmBox.Application.Interfaces.Customer;

namespace OnionCRM.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {

        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public  IActionResult GetAllCustomers()
        {
            var numbers = _customerService.GetAll();
            return Ok(numbers);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            await _customerService.AddAsync(customer);
            return Ok();
        }


        [HttpGet("{id}")]
        public IActionResult GetCustomerById(int id)
        {
            var customer = _customerService.GetById(id);
            return Ok(customer);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(Customer customer)
        {
            _customerService.Update(customer);
            return Ok();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _customerService.DeleteAsync(id);
            return Ok();
        }

    }

}




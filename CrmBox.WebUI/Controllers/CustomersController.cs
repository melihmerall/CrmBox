using CrmBox.Application.Interfaces.Customer;
using CrmBox.Core.Domain;
using CrmBox.Core.Domain.Identity;
using CrmBox.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CrmBox.WebUI.Controllers
{
    [Authorize(Roles = "Admin,Moderator")]

    public class CustomersController : Controller
    {
        readonly ICustomerService _customerService;
        IMemoryCache _memoryCache;
        const string cacheKey = "customerKey";
        public CustomersController(ICustomerService customerService, IMemoryCache memoryCache)
        {
            _customerService = customerService;
            _memoryCache = memoryCache;
        }

        [HttpGet]

        public IActionResult GetAllCustomers()
        {
            IQueryable<Customer> result = _customerService.GetAll();
            if (!_memoryCache.TryGetValue(cacheKey, out object list))
            {

                var cacheExpOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(1),
                    Priority = CacheItemPriority.Normal
                };

                _memoryCache.Set(cacheKey, result, cacheExpOptions);
            }

            return View(result);
        }

        [HttpGet]

        public IActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> AddCustomer(AddCustomerVM model)
        {
            if (ModelState.IsValid)
            {
                await _customerService.AddAsync(new()
                {
                    Address = model.Address,
                    CompanyName = model.CompanyName,
                    CreatedTime = DateTime.Now,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    JobTitle = model.JobTitle,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                });
                return RedirectToAction("GetAllCustomers");

            }

            throw new Exception("Ekleme işlemi esnasında bir hata meydana geldi");
        }

        [HttpGet]

        public IActionResult UpdateCustomer(int id)
        {
            Customer? customer = _customerService.Get(x => x.Id == id);
            if (customer != null)
                return View(customer);
            else
                throw new Exception("Belirtilen id ile eşleşen bir müşteri bulunamadı.");
        }

        [HttpPost]

        public IActionResult UpdateCustomer(Customer model)
        {
            if (ModelState.IsValid)
            {
                _customerService.Update(model);
                return RedirectToAction("GetAllCustomers");
            }
            throw new Exception("Güncelleme işlemi esnasında bir hata meydana geldi");
        }

        [HttpGet]

        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _customerService.DeleteAsync(id);
            return RedirectToAction("GetAllCustomers");
        }
    }
}

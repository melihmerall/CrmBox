using CrmBox.Application.Interfaces.Customer;
using CrmBox.Application.Services.Customer;
using CrmBox.Core.Domain;
using CrmBox.Core.Domain.Identity;
using CrmBox.WebUI.Helper;
using CrmBox.WebUI.Helper.Twilio;
using CrmBox.WebUI.Models;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using TwilioSettings = CrmBox.WebUI.Helper.Twilio.TwilioSettings;

namespace CrmBox.WebUI.Controllers
{

    public class CustomersController : Controller
    {
        readonly ICustomerService _customerService;
        IMemoryCache _memoryCache;
        readonly TwilioSettings _twilioOptions;
        const string cacheKey = "customerKey";
        private readonly EmailHelper _emailHelper;
        public CustomersController(ICustomerService customerService, IMemoryCache memoryCache, IOptions<TwilioSettings> twilioOptions, EmailHelper emailHelper)
        {
            _customerService = customerService;
            _memoryCache = memoryCache;
            _twilioOptions = twilioOptions.Value;
            _emailHelper = emailHelper;
        }

        [HttpGet]
        [Authorize(Policy = "GetAllCustomers")]
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
        [Authorize(Policy = "AddCustomer")]
        public IActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "AddCustomer")]
        public async Task<IActionResult> AddCustomer(AddCustomerVM model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer
                {
                    Address = model.Address,
                    CompanyName = model.CompanyName,
                    CreatedTime = DateTime.Now,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    JobTitle = model.JobTitle,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                };

                await _customerService.AddAsync(customer);


                return RedirectToAction("GetAllCustomers");

            }
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "UpdateCustomer")]
        public IActionResult UpdateCustomer(int id)
        {
            var values = _customerService.GetById(id);
            AddCustomerVM model = new AddCustomerVM
            {
                Address = values.Address,
                CompanyName = values.CompanyName,
                Email = values.Email,
                FirstName = values.FirstName,
                JobTitle = values.JobTitle,
                LastName = values.LastName,
                PhoneNumber = values.PhoneNumber,
            };
            return View(model);

        }

        [HttpPost]
        [Authorize(Policy = "UpdateCustomer")]
        public IActionResult UpdateCustomer(AddCustomerVM model)
        {
            var values = _customerService.GetById(model.Id);
            {
                values.Address = model.Address;
                values.CompanyName = model.CompanyName;
                values.CreatedTime = DateTime.Now;
                values.Email = model.Email;
                values.FirstName = model.FirstName;
                values.JobTitle = model.JobTitle;
                values.LastName = model.LastName;
                values.PhoneNumber = model.PhoneNumber;

            };
            if (ModelState.IsValid)
            {
                _customerService.Update(values);
                return RedirectToAction("GetAllCustomers");


            }
            return View();


        }

        [HttpGet]
        [Authorize(Policy = "DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _customerService.DeleteAsync(id);
            return RedirectToAction("GetAllCustomers");
        }

        [HttpGet]
        [Authorize(Policy = "SendSms")]
        public async Task<IActionResult> SendSms(int id)
        {

            var values = _customerService.GetById(id);

            ViewBag.phoneNumber = values.PhoneNumber;
            SendSmsVM model = new SendSmsVM
            {
                Id = values.Id,
                PhoneNumber = values.PhoneNumber
            };

            return View(model);
        }
        [HttpPost]
        [Authorize(Policy = "SendSms")]
        public async Task<IActionResult> SendSms(SendSmsVM vm)
        {
            TwilioClient.Init(_twilioOptions.AccountSid, _twilioOptions.AuthToken);
            var values = _customerService.GetAll().Where(x => x.Id == vm.Id).FirstOrDefault();
            {
                values.Id = vm.Id;
                values.PhoneNumber = vm.PhoneNumber;
                

            };
            try
            {
                if (ModelState.IsValid)
                {
                    var message = MessageResource.Create(
                   body: "Messages: " + vm.MessageBody,
                   from: new PhoneNumber(_twilioOptions.PhoneNumber),
                   to: new PhoneNumber(vm.PhoneNumber));

                    ViewBag.State = true;
                }

            }
            catch (Exception ex)
            {

                ViewBag.State = false;
                throw;
            }
            return View();
        }
        [HttpGet]
        public IActionResult SendMail()
        {
            //var values = _customerService.GetById(id);


            //SendMailVM model = new SendMailVM
            //{
            //    Id = values.Id,
            //    Mail = values.Email,

            //};
            return View();
        }

        [HttpPost]
        public IActionResult SendMail(SendMailVM model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = _customerService.GetAll().Where(x => x.Id == model.Id).FirstOrDefault();
                if (customer != null)
                {
                    try
                    {

                     
                        _emailHelper.SendEmail(model.Mail, model.Messages,model.Subject);
                        ViewBag.State = true;
                    }
                    catch (Exception ex)
                    {

                        ViewBag.State = false;
                        throw;
                    }
                }
                
            }
            return View();
        }



    }
}


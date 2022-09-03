﻿using CrmBox.Application.Interfaces.Customer;
using CrmBox.Application.Services.Customer;
using CrmBox.Core.Domain;
using CrmBox.Core.Domain.Identity;
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

namespace CrmBox.WebUI.Controllers
{
    [Authorize(Roles = "Root,Admin,Moderator")]
    public class CustomersController : Controller
    {
        readonly ICustomerService _customerService;
        IMemoryCache _memoryCache;
        readonly TwilioSettings _twilioOptions;
        const string cacheKey = "customerKey";
        public CustomersController(ICustomerService customerService, IMemoryCache memoryCache, IOptions<TwilioSettings> twilioOptions)
        {
            _customerService = customerService;
            _memoryCache = memoryCache;
            _twilioOptions = twilioOptions.Value;
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
                var message = MessageResource.Create(
                    body: "Messages" + vm.MessageBody,
                    from: new Twilio.Types.PhoneNumber(_twilioOptions.PhoneNumber),
                    to: new Twilio.Types.PhoneNumber(vm.PhoneNumber));
            }
            catch (Exception ex)
            {

                throw;
            }
            return View();
        }
    }
}

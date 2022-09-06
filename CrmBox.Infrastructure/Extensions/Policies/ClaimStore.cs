using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Localization;
namespace CrmBox.Infrastructure.Extensions.Policies
{
    public static class ClaimStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            //AppUserController
            new Claim("Get All Users","Get All Users"),
            new Claim("Add User","Add User"),
            new Claim("Update User","Update User"),
            new Claim("Delete User","Delete User"),

            //AppRolesController
            new Claim("Get All User Roles","Get All User Roles"),
            new Claim("Add User Role","Add User Role"),
            new Claim("Update User Role","Update User Role"),
            new Claim("Delete User Role","Delete User Role"),
            new Claim("Choose User Role","Choose User Role"),
            new Claim("Manage User Claims","Manage User Claims"),

            //CustomerController
            new Claim("Get All Customers","Get All Customers"),
            new Claim("Add Customer","Add Customer"),
            new Claim("Update Customer","Update Customer"),
            new Claim("Delete Customer","Delete Customer"),
            new Claim("Send Sms","Send Sms"),

            //ChatController
            new Claim("Chat","Chat"),


        };
    }
}

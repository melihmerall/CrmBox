using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmBox.Infrastructure.Extensions.Policies
{
    public static class PolicyTypes
    {
        public static List<string> Policies { get; } = new()
        {
            "GetAllUsers",
            "AddUser",
            "DeleteUser",

            "GetAllUserRoles",
            "AddUserRole",
            "UpdateUserRole",
            "DeleteUserRole",
            "AssignUserRole",

            "GetAllCustomers",
            "AddCustomer",
            "UpdateCustomer",
            "DeleteCustomer"
        };
    }
}

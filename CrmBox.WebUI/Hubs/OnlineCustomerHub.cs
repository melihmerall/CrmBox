using Microsoft.AspNetCore.SignalR;

namespace CrmBox.WebUI.Hubs
{
    public class OnlineCustomerHub : Hub
    {

        public static int customerCounter = 0;
        public static int customerCounterCount = 0;

        public void SendCustomerCounter()
        {
            Clients.All.SendAsync("GetCustomerCounter", customerCounterCount.ToString());
        }
        public override Task OnConnectedAsync()
        {

            customerCounter++;
            customerCounterCount = customerCounter - UserOnlineHub.userCounter;
            if (customerCounterCount > 0)
            {
                SendCustomerCounter();
            }
            else
            {
                Console.WriteLine("0'dan küçük olamaz.");
            }

            

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {

            customerCounterCount--;
            

            SendCustomerCounter();
            return base.OnDisconnectedAsync(exception);
        }
    }
}

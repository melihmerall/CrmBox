using Microsoft.AspNetCore.SignalR;

namespace CrmBox.WebUI.Hubs
{
    public class UserOnlineHub:Hub
    {


        public static int userCounter=0;
   
        public void SendUserCounter()
        {
            
            Clients.All.SendAsync("GetUserCounter", userCounter.ToString());
        }


        public override Task OnConnectedAsync()
        {
            userCounter++;
            

            SendUserCounter();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            userCounter--;
            
            SendUserCounter();

            return base.OnDisconnectedAsync(exception);
        }
    }
}

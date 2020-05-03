using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace PayrollProcessor.Api.Features.Notifications
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> hub;

        public NotificationController(IHubContext<NotificationHub> hub)
        {
            if (hub is null)
            {
                throw new ArgumentNullException(nameof(hub));
            }

            this.hub = hub;
        }

        [HttpPost]
        public async Task<ActionResult> PostMessage([FromBody] Notification notification)
        {
            await hub.Clients.All.SendAsync("received", notification);

            return Ok();
        }
    }
}

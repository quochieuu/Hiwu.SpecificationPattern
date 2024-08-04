using Hiwu.SpecificationPattern.Generic;
using Hiwu.SpecificationPattern.SignalR.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Hiwu.SpecificationPattern.SampleApi.Controllers
{
    public class NotificationController : BaseApiController
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationController(IHubContext<NotificationHub> hubContext, IUnitOfWork unitOfWork)
        {
            _hubContext = hubContext;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("broadcast")]
        public async Task<IActionResult> BroadcastNotification(string message)
        {
            var notificationHub = new NotificationHub(new Logger<NotificationHub>(new LoggerFactory()), _unitOfWork);
            await notificationHub.BroadcastMessage(message);
            return Ok();
        }

        [HttpPost("private")]
        public async Task<IActionResult> SendPrivateNotification(string userId, string notificationMessage)
        {
            var notificationHub = new NotificationHub(new Logger<NotificationHub>(new LoggerFactory()), _unitOfWork);
            await notificationHub.SendPrivateNotification(userId, notificationMessage);
            return Ok();
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> SubscribeToChannel(string userId, string channelName)
        {
            var notificationHub = new NotificationHub(new Logger<NotificationHub>(new LoggerFactory()), _unitOfWork);
            await notificationHub.SubscribeToChannel(userId, channelName);
            return Ok();
        }

        [HttpPost("unsubscribe")]
        public async Task<IActionResult> UnsubscribeFromChannel(string userId, string channelName)
        {
            var notificationHub = new NotificationHub(new Logger<NotificationHub>(new LoggerFactory()), _unitOfWork);
            await notificationHub.UnsubscribeFromChannel(userId, channelName);
            return Ok();
        }

        [HttpPost("channel")]
        public async Task<IActionResult> SendChannelNotification(string channelName, string notificationMessage)
        {
            var notificationHub = new NotificationHub(new Logger<NotificationHub>(new LoggerFactory()), _unitOfWork);
            await notificationHub.SendChannelNotification(channelName, notificationMessage);
            return Ok();
        }
    }
}

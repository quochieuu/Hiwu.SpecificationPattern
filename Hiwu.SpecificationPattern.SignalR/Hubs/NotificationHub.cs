using Hiwu.SpecificationPattern.Generic;
using Microsoft.Extensions.Logging;

namespace Hiwu.SpecificationPattern.SignalR.Hubs
{
    public class NotificationHub : BaseHub
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationHub(ILogger<BaseHub> logger, IUnitOfWork unitOfWork) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }

        public override async Task BroadcastMessage(string message)
        {
            await SendMessageToAll(message);
        }

        public async Task SendPrivateNotification(string userId, string notificationMessage)
        {
            //var connectionId = await _unitOfWork.UserRepository.GetConnectionIdByUserIdAsync(userId);
            //if (connectionId != null)
            //{
            //    await SendMessageToClient(connectionId, notificationMessage);
            //}
        }

        public async Task SubscribeToChannel(string userId, string channelName)
        {
            //var connectionId = await _unitOfWork.UserRepository.GetConnectionIdByUserIdAsync(userId);
            //if (connectionId != null)
            //{
            //    await AddToGroup(channelName);
            //    await SendMessageToClient(connectionId, $"Subscribed to channel {channelName}.");
            //}
        }

        public async Task UnsubscribeFromChannel(string userId, string channelName)
        {
            //var connectionId = await _unitOfWork.UserRepository.GetConnectionIdByUserIdAsync(userId);
            //if (connectionId != null)
            //{
            //    await RemoveFromGroup(channelName);
            //    await SendMessageToClient(connectionId, $"Unsubscribed from channel {channelName}.");
            //}
        }

        public async Task SendChannelNotification(string channelName, string notificationMessage)
        {
            await SendMessageToGroup(channelName, notificationMessage);
        }
    }
}

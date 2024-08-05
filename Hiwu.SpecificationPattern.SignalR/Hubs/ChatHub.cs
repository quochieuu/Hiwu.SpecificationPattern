using Hiwu.SpecificationPattern.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Hiwu.SpecificationPattern.SignalR.Hubs
{
    public class ChatHub : BaseHub
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChatHub(ILogger<BaseHub> logger, IUnitOfWork unitOfWork) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }

        public override async Task BroadcastMessage(string message)
        {
            await SendMessageToAll(message);
        }

        public async Task SendPrivateMessage(string userId, string message)
        {
            //var connectionId = await _unitOfWork.UserRepository.GetConnectionIdByUserIdAsync(userId);
            //if (connectionId != null)
            //{
            //    await SendMessageToClient(connectionId, message);
            //}
        }

        public async Task JoinRoom(string userId, string roomName)
        {
            //var connectionId = await _unitOfWork.UserRepository.GetConnectionIdByUserIdAsync(userId);
            //if (connectionId != null)
            //{
            //    await AddToGroup(roomName);
            //    await SendMessageToGroup(roomName, $"{userId} has joined the room {roomName}.");
            //}
        }

        public async Task LeaveRoom(string userId, string roomName)
        {
            //var connectionId = await _unitOfWork.UserRepository.GetConnectionIdByUserIdAsync(userId);
            //if (connectionId != null)
            //{
            //    await RemoveFromGroup(roomName);
            //    await SendMessageToGroup(roomName, $"{userId} has left the room {roomName}.");
            //}
        }

        public async Task<List<string>> GetConnectedUsersInRoom(string roomName)
        {
            // This is a placeholder for getting a list of connected users in a specific room.
            // You would need to implement this method to retrieve actual data.
            return await Task.FromResult(new List<string> { "User1", "User2" });
        }
    }
}

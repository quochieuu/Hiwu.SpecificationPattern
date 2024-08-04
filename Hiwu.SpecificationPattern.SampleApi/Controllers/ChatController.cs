using Hiwu.SpecificationPattern.Generic;
using Hiwu.SpecificationPattern.SignalR.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Hiwu.SpecificationPattern.SampleApi.Controllers
{
    public class ChatController : BaseApiController
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;

        public ChatController(IHubContext<ChatHub> hubContext, IUnitOfWork unitOfWork)
        {
            _hubContext = hubContext;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("broadcast")]
        public async Task<IActionResult> BroadcastMessage(string message)
        {
            var chatHub = new ChatHub(new Logger<ChatHub>(new LoggerFactory()), _unitOfWork);
            await chatHub.BroadcastMessage(message);
            return Ok();
        }

        [HttpPost("private")]
        public async Task<IActionResult> SendPrivateMessage(string userId, string message)
        {
            var chatHub = new ChatHub(new Logger<ChatHub>(new LoggerFactory()), _unitOfWork);
            await chatHub.SendPrivateMessage(userId, message);
            return Ok();
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinRoom(string userId, string roomName)
        {
            var chatHub = new ChatHub(new Logger<ChatHub>(new LoggerFactory()), _unitOfWork);
            await chatHub.JoinRoom(userId, roomName);
            return Ok();
        }

        [HttpPost("leave")]
        public async Task<IActionResult> LeaveRoom(string userId, string roomName)
        {
            var chatHub = new ChatHub(new Logger<ChatHub>(new LoggerFactory()), _unitOfWork);
            await chatHub.LeaveRoom(userId, roomName);
            return Ok();
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetConnectedUsersInRoom(string roomName)
        {
            var chatHub = new ChatHub(new Logger<ChatHub>(new LoggerFactory()), _unitOfWork);
            var users = await chatHub.GetConnectedUsersInRoom(roomName);
            return Ok(users);
        }
    }
}

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Hiwu.SpecificationPattern.SignalR.Hubs
{
    public abstract class BaseHub : Hub
    {
        protected readonly ILogger<BaseHub> _logger;

        // Constructor to initialize the logger
        public BaseHub(ILogger<BaseHub> logger)
        {
            _logger = logger;
        }

        // Send a message to all connected clients
        public async Task SendMessageToAll(string message)
        {
            _logger.LogInformation("Sending message to all clients: {Message}", message);
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        // Send a message to a specific client by connection ID
        public async Task SendMessageToClient(string connectionId, string message)
        {
            _logger.LogInformation("Sending message to client {ConnectionId}: {Message}", connectionId, message);
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        }

        // Send a message to a specific group of clients
        public async Task SendMessageToGroup(string groupName, string message)
        {
            _logger.LogInformation("Sending message to group {GroupName}: {Message}", groupName, message);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }

        // Add a client to a specific group
        public async Task AddToGroup(string groupName)
        {
            var connectionId = Context.ConnectionId;
            _logger.LogInformation("Adding connection {ConnectionId} to group {GroupName}", connectionId, groupName);
            await Groups.AddToGroupAsync(connectionId, groupName);
        }

        // Remove a client from a specific group
        public async Task RemoveFromGroup(string groupName)
        {
            var connectionId = Context.ConnectionId;
            _logger.LogInformation("Removing connection {ConnectionId} from group {GroupName}", connectionId, groupName);
            await Groups.RemoveFromGroupAsync(connectionId, groupName);
        }

        // Abstract method that must be implemented by derived Hubs to broadcast a message
        public abstract Task BroadcastMessage(string message);

        // Called when a client connects to the Hub
        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        // Called when a client disconnects from the Hub
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
            if (exception != null)
            {
                _logger.LogError(exception, "Exception occurred while disconnecting.");
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}

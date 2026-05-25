using Microsoft.AspNetCore.SignalR;

public class LavenderFlowHub : Hub {
    public async Task JoinBoard(int boardId)
    {
        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            boardId.ToString());
    }

    public async Task LeaveBoard(int boardId)
    {
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            boardId.ToString());
    }
}
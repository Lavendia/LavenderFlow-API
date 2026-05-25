public class BoardUserResponse
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int BoardId { get; set; }
    public string? RoleName { get; set; }
    public DateTime CreatedAt { get; set; }

    public BoardUserResponse(BoardUser bu)
    {
        UserId = bu.UserId;
        UserName = bu.User?.Name ?? "";
        BoardId = bu.BoardId;
        RoleName = bu.BoardRole?.Name;
        CreatedAt = bu.CreatedAt;
    }
}
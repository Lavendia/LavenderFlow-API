using System.Diagnostics.CodeAnalysis;

public class BoardUser : BaseEntity
{
    public int UserId { get; set; }
    public User? User { get; set; }

    public int BoardId { get; set; }
    public Board? Board { get; set; }

    public int BoardRoleId { get; set; }
    public BoardRole? BoardRole { get; set; }

    public BoardUser() {}

    [SetsRequiredMembers]
    public BoardUser(int userId, int boardId, int boardRoleId)
    {
        UserId = userId;
        BoardId = boardId;
        BoardRoleId = boardRoleId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
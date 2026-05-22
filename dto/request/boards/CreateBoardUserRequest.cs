public class CreateBoardUserRequest
{
    public required int UserId { get; set; }
    public required int BoardId { get; set; }
    public required int BoardRoleId { get; set; }
}
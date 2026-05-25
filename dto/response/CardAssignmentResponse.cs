public class CardAssignmentResponse
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int CardId { get; set; }
    public DateTime CreatedAt { get; set; }

    public CardAssignmentResponse(CardAssignment ca)
    {
        UserId = ca.UserId;
        UserName = ca.User?.Name ?? "";
        CardId = ca.CardId;
        CreatedAt = ca.CreatedAt;
    }
}
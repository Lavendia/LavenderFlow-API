public class CreateChatMessageRequest
{
    public required string Text { get; set; }
    public required int CardId { get; set; }
    public required int UserId { get; set; }
}

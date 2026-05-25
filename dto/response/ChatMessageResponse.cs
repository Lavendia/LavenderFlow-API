public class ChatMessageResponse
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int CardId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ChatMessageResponse(ChatMessage message)
    {
        Id = message.Id;
        Text = message.Text;
        CardId = message.CardId;
        UserId = message.UserId;
        CreatedAt = message.CreatedAt;
        UpdatedAt = message.UpdatedAt;
    }
}

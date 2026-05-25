using System.Diagnostics.CodeAnalysis;

public class ChatMessage : BaseEntity
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public int CardId { get; set; }
    public Card? Card { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }


    public ChatMessage() {}

    [SetsRequiredMembers]
    public ChatMessage(string text, int cardId, int userId)
    {
        Text = text;
        CardId = cardId;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
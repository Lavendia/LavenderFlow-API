using System.Diagnostics.CodeAnalysis;

public class CardAssignment : BaseEntity
{
    public int UserId { get; set; }
    public User? User { get; set; }

    public int CardId { get; set; }
    public Card? Card { get; set; }

    public CardAssignment() {}

    [SetsRequiredMembers]
    public CardAssignment(int userId, int cardId)
    {
        UserId = userId;
        CardId = cardId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
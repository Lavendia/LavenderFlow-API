using System.Diagnostics.CodeAnalysis;

public class CardLabel : BaseEntity
{
    public int CardId { get; set; }
    public Card? Card { get; set; }

    public int LabelId { get; set; }
    public Label? Label { get; set; }

    public CardLabel() {}

    [SetsRequiredMembers]
    public CardLabel(int cardId, int labelId)
    {
        CardId = cardId;
        LabelId = labelId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}

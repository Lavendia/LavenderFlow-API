public class CardLabelResponse
{
    public int CardId { get; set; }
    public int LabelId { get; set; }
    public DateTime CreatedAt { get; set; }

    public CardLabelResponse(CardLabel cardLabel)
    {
        CardId = cardLabel.CardId;
        LabelId = cardLabel.LabelId;
        CreatedAt = cardLabel.CreatedAt;
    }
}

public class LabelResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ColorHex { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public LabelResponse(Label label)
    {
        Id = label.Id;
        Name = label.Name;
        ColorHex = label.ColorHex;
        CreatedAt = label.CreatedAt;
        UpdatedAt = label.UpdatedAt;
    }
}

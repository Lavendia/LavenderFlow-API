using System.Diagnostics.CodeAnalysis;

public class Label : BaseEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string ColorHex { get; set; }

    public Label() {}

    [SetsRequiredMembers]
    public Label(string name, string colorHex)
    {
        Name = name;
        ColorHex = colorHex;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
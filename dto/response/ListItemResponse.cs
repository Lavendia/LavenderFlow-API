public class ListItemResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public int BoardId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ListItemResponse(ListItem listItem)
    {
        Id = listItem.Id;
        Name = listItem.Name;
        Order = listItem.Order;
        BoardId = listItem.BoardId;
        CreatedAt = listItem.CreatedAt;
        UpdatedAt = listItem.UpdatedAt;
    }
}
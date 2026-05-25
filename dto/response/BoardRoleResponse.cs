public class BoardRoleResponse
{
    public int Id { get; set; }
    public string Name { get; set; }

    public BoardRoleResponse(BoardRole role)
    {
        Id = role.Id;
        Name = role.Name;
    }
}
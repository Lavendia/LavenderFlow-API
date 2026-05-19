public class CreateBoardRequest
{
    public required string Name {get; set;}
    public string? Description {get; set;}

    public required int WorkspaceId {get; set;}
}
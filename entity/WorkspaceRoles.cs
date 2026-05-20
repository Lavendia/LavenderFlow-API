using System.Diagnostics.CodeAnalysis;
public class WorkspacesRoles : BaseEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }

    [SetsRequiredMembers]
    public WorkspacesRoles(string name)
    {
        Name = name;
    }
}


public static class Seeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!context.BoardRoles.Any())
        {
            context.BoardRoles.AddRange(
                new BoardRole("Admin"),
                new BoardRole("Editor"),
                new BoardRole("Viewer")
            );
        }

        if (!context.WorkspaceRoles.Any())
        {
            context.WorkspaceRoles.AddRange(
                new WorkspaceRole("Admin"),
                new WorkspaceRole("Editor"),
                new WorkspaceRole("Viewer")
            );
        }

        await context.SaveChangesAsync();
    }
}
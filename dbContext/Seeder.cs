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

        if (!context.Labels.Any())
        {
            context.Labels.AddRange(
                new Label("High", "#EF4444"),
                new Label("Medium", "#FBBF24"),
                new Label("Low", "#10B981")
            );
        }

        await context.SaveChangesAsync();
    }
}
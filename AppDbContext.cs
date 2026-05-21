using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Workspace> Workspaces { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<BoardRole> BoardRoles { get; set; }
    public DbSet<BoardUser> BoardUsers { get; set; }
    public DbSet<ListItem> ListItems { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Checklist> Checklists { get; set; }
    public DbSet<ChecklistItem> ChecklistItems { get; set; }
    public DbSet<CardAssignment> CardAssignments { get; set; }
    public DbSet<WorkspacesRoles> Roles { get; set; }
    public DbSet<WorkspacesUsers> WorkspaceUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<BoardUser>()
            .HasKey(bu => new { bu.UserId, bu.BoardId });

        modelBuilder.Entity<BoardUser>()
            .HasOne(bu => bu.User)
            .WithMany()
            .HasForeignKey(bu => bu.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardUser>()
            .HasOne(bu => bu.Board)
            .WithMany()
            .HasForeignKey(bu => bu.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardUser>()
            .HasOne(bu => bu.BoardRole)
            .WithMany()
            .HasForeignKey(bu => bu.BoardRoleId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Board>()
            .HasOne(b => b.Workspace)
            .WithMany()
            .HasForeignKey(b => b.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ListItem>()
            .HasOne(l => l.Board)
            .WithMany()
            .HasForeignKey(l => l.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Card>()
            .HasOne(c => c.ListItem)
            .WithMany()
            .HasForeignKey(c => c.ListItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CardAssignment>()
            .HasKey(ca => new { ca.UserId, ca.CardId }); // composite PK

        modelBuilder.Entity<CardAssignment>()
            .HasOne(ca => ca.User)
            .WithMany()
            .HasForeignKey(ca => ca.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CardAssignment>()
            .HasOne(ca => ca.Card)
            .WithMany()
            .HasForeignKey(ca => ca.CardId);
        modelBuilder.Entity<Checklist>()
            .HasOne(c => c.Card)
            .WithMany()
            .HasForeignKey(c => c.CardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChecklistItem>()
            .HasOne(ci => ci.Checklist)
            .WithMany(c => c.Items)
            .HasForeignKey(ci => ci.ChecklistId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
            entry.Entity.UpdatedAt = DateTime.UtcNow;
    }
}
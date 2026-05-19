using System.ComponentModel.DataAnnotations;

public class BaseEntity {
    public required DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public required DateTime UpdatedAt {get; set;} = DateTime.UtcNow;
}
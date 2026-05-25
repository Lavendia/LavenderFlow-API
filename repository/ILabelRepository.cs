public interface ILabelRepository
{
    Task<List<Label>> GetAllAsync();
    Task<Label?> GetByIdAsync(int id);
    void Add(Label label);
    void Delete(Label label);
    Task SaveAsync();
}

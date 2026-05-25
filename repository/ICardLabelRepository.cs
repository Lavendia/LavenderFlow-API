public interface ICardLabelRepository
{
    Task<List<CardLabel>> GetAllAsync();
    Task<CardLabel?> GetByIdsAsync(int cardId, int labelId);
    Task<List<CardLabel>> GetByCardIdAsync(int cardId);
    Task<List<CardLabel>> GetByLabelIdAsync(int labelId);
    Task<bool> ExistsAsync(int cardId, int labelId);
    void Add(CardLabel cardLabel);
    void Delete(CardLabel cardLabel);
    Task SaveAsync();
}

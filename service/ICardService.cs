public interface ICardService
{
    Task<IEnumerable<CardResponse>> GetCardsAsync();
    Task<CardResponse?> GetCardAsync(int id);
    Task<CardResponse?> CreateCardAsync(CreateCardRequest request);
    Task<CardResponse?> UpdateCardAsync(int id, UpdateCardRequest request);
    Task<bool> DeleteCardAsync(int id);
}

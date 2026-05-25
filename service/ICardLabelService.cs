public interface ICardLabelService
{
    Task<IEnumerable<CardLabelResponse>> GetCardLabelsAsync();
    Task<IEnumerable<LabelResponse>> GetLabelsByCardIdAsync(int cardId);
    Task<IEnumerable<CardLabelResponse>> GetCardsByLabelIdAsync(int labelId);
    Task<CardLabelResponse?> AddLabelToCardAsync(CreateCardLabelRequest request);
    Task<bool> RemoveLabelFromCardAsync(int cardId, int labelId);
}

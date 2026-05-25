public interface ILabelService
{
    Task<IEnumerable<LabelResponse>> GetLabelsAsync();
    Task<LabelResponse?> GetLabelAsync(int id);
    Task<LabelResponse?> CreateLabelAsync(CreateLabelRequest request);
    Task<LabelResponse?> UpdateLabelAsync(int id, UpdateLabelRequest request);
    Task<bool> DeleteLabelAsync(int id);
}

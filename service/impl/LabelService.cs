using Microsoft.AspNetCore.SignalR;

public class LabelService : ILabelService
{
    private readonly ILabelRepository _repository;
    private readonly IHubContext<LavenderFlowHub> _hub;

    public LabelService(
        ILabelRepository repository,
        IHubContext<LavenderFlowHub> hub)
    {
        _repository = repository;
        _hub = hub;
    }

    public async Task<IEnumerable<LabelResponse>> GetLabelsAsync()
    {
        var labels = await _repository.GetAllAsync();
        return labels.Select(l => new LabelResponse(l));
    }

    public async Task<LabelResponse?> GetLabelAsync(int id)
    {
        var label = await _repository.GetByIdAsync(id);
        return label == null ? null : new LabelResponse(label);
    }

    public async Task<LabelResponse?> CreateLabelAsync(CreateLabelRequest request)
    {
        var label = new Label(request.Name, request.ColorHex);
        _repository.Add(label);
        await _repository.SaveAsync();
        var response = new LabelResponse(label);

        await _hub.Clients.All.SendAsync("LabelCreated", response);

        return response;
    }

    public async Task<LabelResponse?> UpdateLabelAsync(int id, UpdateLabelRequest request)
    {
        var label = await _repository.GetByIdAsync(id);
        if (label == null)
            return null;

        if (request.Name is not null) label.Name = request.Name;
        if (request.ColorHex is not null) label.ColorHex = request.ColorHex;

        await _repository.SaveAsync();
        var response = new LabelResponse(label);

        await _hub.Clients.All.SendAsync("LabelUpdated", response);

        return response;
    }

    public async Task<bool> DeleteLabelAsync(int id)
    {
        var label = await _repository.GetByIdAsync(id);
        if (label == null)
            return false;

        _repository.Delete(label);
        await _repository.SaveAsync();

        await _hub.Clients.All.SendAsync("LabelDeleted", id);

        return true;
    }
}

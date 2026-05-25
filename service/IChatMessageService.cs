public interface IChatMessageService
{
    Task<IEnumerable<ChatMessageResponse>> GetChatMessagesAsync();
    Task<ChatMessageResponse?> GetChatMessageAsync(int id);
    Task<IEnumerable<ChatMessageResponse>> GetChatMessagesByCardIdAsync(int cardId);
    Task<ChatMessageResponse?> CreateChatMessageAsync(CreateChatMessageRequest request);
    Task<ChatMessageResponse?> UpdateChatMessageAsync(int id, UpdateChatMessageRequest request);
    Task<bool> DeleteChatMessageAsync(int id);
}

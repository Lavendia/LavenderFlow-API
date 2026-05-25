using Microsoft.EntityFrameworkCore;

public class ChatMessageRepository : IChatMessageRepository
{
    private readonly AppDbContext _context;

    public ChatMessageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ChatMessage>> GetAllAsync()
    {
        return await _context.ChatMessages.ToListAsync();
    }

    public async Task<ChatMessage?> GetByIdAsync(int id)
    {
        return await _context.ChatMessages.FindAsync(id);
    }

    public async Task<List<ChatMessage>> GetByCardIdAsync(int cardId)
    {
        return await _context.ChatMessages.Where(m => m.CardId == cardId).ToListAsync();
    }

    public void Add(ChatMessage message)
    {
        _context.ChatMessages.Add(message);
    }

    public void Delete(ChatMessage message)
    {
        _context.ChatMessages.Remove(message);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}

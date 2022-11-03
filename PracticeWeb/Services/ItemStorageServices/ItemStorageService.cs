using Microsoft.EntityFrameworkCore;

using PracticeWeb.Models;

namespace PracticeWeb.Services.ItemStorageServices;

public class ItemStorageService : IItemStorageService
{
    private Context _context;
    private CommonQueries<Item> _common;

    public ItemStorageService(Context context)
    {
        _context = context;
        _common = new CommonQueries<Item>(_context);
    }

    public async Task<Item> CreateAsync(Item entity) =>
        await _common.CreateAsync(entity);

    public async Task<Item?> GetAsync(string id) =>
        await _common.GetAsync(id, _context.Items);

    public async Task<List<Item>> GetAllAsync() =>
        await _common.GetAllAsync(_context.Items);

    public async Task<ItemType?> GetItemType(int id) =>
        await _context.ItemTypes.FirstOrDefaultAsync(t => t.Id == id);

    public async Task UpdateAsync(string id, Item entity) =>
        await _common.UpdateAsync(id, entity);

    public async Task DeleteAsync(string id) =>
        await _common.DeleteAsync(id);
}
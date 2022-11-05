using Microsoft.EntityFrameworkCore;

using PracticeWeb.Models;

namespace PracticeWeb.Services.ItemStorageServices;

public class ItemStorageService : IItemStorageService
{
    private Context _context;
    private CommonQueries<Item> _common;
    private CommonQueries<FileEntity> _commonFile;

    public ItemStorageService(Context context)
    {
        _context = context;
        _common = new CommonQueries<Item>(_context);
        _commonFile = new CommonQueries<FileEntity>(_context);
    }

    public async Task<Item> CreateAsync(Item entity) =>
        await _common.CreateAsync(entity);

    public async Task<Item?> GetAsync(string id) =>
        await _common.GetAsync(id, IncludeValues());

    public async Task<List<Item>> GetAllAsync() =>
        await _common.GetAllAsync(IncludeValues());

    public async Task<ItemType?> GetItemTypeAsync(int id) =>
        await _context.ItemTypes.FirstOrDefaultAsync(t => t.Id == id);

    public async Task UpdateAsync(string id, Item entity) =>
        await _common.UpdateAsync(id, entity);

    public async Task DeleteAsync(string id) =>
        await _common.DeleteAsync(id);

    public async Task CreateFileAsync(FileEntity entity) =>
        await _commonFile.CreateAsync(entity);

    public async Task<FileEntity?> GetFileAsync(string id) =>
        await _commonFile.GetAsync(id, _context.Files);

    public async Task DeleteFileAsync(string id) =>
        await _commonFile.DeleteAsync(id);

    public async Task CreateConnectionAsync(Connection entity)
    {
        await _context.Connections.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Connection>> GetConnectionsByParentAsync(string parentId) =>
        await IncludeConnectionValues().Where(c => c.ParentId == parentId).ToListAsync();

    public async Task<Connection?> GetConnectionByChildAsync(string childId) =>
        await IncludeConnectionValues().FirstOrDefaultAsync(c => c.ChildId == childId);

    public async Task DeleteConnectionAsync(string parentId, string childId)
    {
        var connection = GetConnectionsByParentAsync(parentId);
        if (connection == null)
            throw new NullReferenceException();
        _context.Remove(connection);
        await _context.SaveChangesAsync();
    }

    private IQueryable<Item> IncludeValues() =>
        _context.Items
            .Include(e => e.Type);

    private IQueryable<Connection> IncludeConnectionValues() =>
        _context.Connections
            .Include(c => c.Child)
            .Include(c => c.Parent);
}
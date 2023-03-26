using Microsoft.EntityFrameworkCore;
using PracticeWeb.Exceptions;
using PracticeWeb.Models;

namespace PracticeWeb.Services.FileSystemServices.Helpers;

public class TaskHelperService : FileSystemQueriesHelper, IFileSystemHelper
{
    private CommonQueries<string, TaskEntity> _commonTaskQueries;
    private CommonQueries<string, FileEntity> _commonFileQueries;
    private CommonQueries<string, Work> _commonWorkQueries;

    public TaskHelperService(
        IHostEnvironment env,
        ServiceResolver serviceAccessor,
        Context context) : base(env, serviceAccessor, context)
    {
        _commonTaskQueries = new CommonQueries<string, TaskEntity>(_context);
        _commonFileQueries = new CommonQueries<string, FileEntity>(_context);
        _commonWorkQueries = new CommonQueries<string, Work>(_context);
    }

    public async Task<ItemAccess> HasAccessAsync(string id, User user, List<string> path)
    {
        var access = await HasUserAccessToParentAsync(id, user, path);
        Console.WriteLine($"task access: {access.Permission} in {id}");
        access.Path.Add(id);
        return access;
    }

    public async Task<object> GetAsync(string id, User user, Boolean asChild)
    {
        var access = await HasAccessAsync(id, user, new List<string>());
        var item = await TryGetItemAsync(id);
        var task = await _commonTaskQueries.GetAsync(id, _context.Tasks);
        var folder = await base.GetFolderAsync(id, user, asChild);

        var workConnection = await _context.Connections
            .Include(c => c.Child)
            .FirstOrDefaultAsync(c => c.ParentId == id && c.Child.CreatorId == user.Id);

        if (user.RoleId != UserRole.Student)
            folder.Access.Remove("Work");

        return new
        {
            Name = folder.Name,
            Type = folder.Type,
            Guid = folder.Guid,
            Path = folder.Path,
            Children = asChild ? null : folder.Children,
            Data = new {
                CreationTime = folder.Data.CreationTime,
                CreatorName = folder.Data.CreatorName,
                Until = task?.Until,
                Work = workConnection != null ? await GetWorkData(workConnection.ChildId, user) : null,
            },
            Access = folder.Access,
            IsEditable = folder.IsEditable
        };
    }

    public async Task CheckIfCanCreateAsync(string parentId, User user)
    {
        if (parentId == _rootGuid)
            throw new InvalidPathException();

        await base.CheckIfCanCreateAsync(parentId, Type.Task, user);
    }

    public async Task<(string, object)> CreateAsync(string parentId, string name, User user, Dictionary<string, object>? parameters=null)
    {
        await CheckIfCanCreateAsync(parentId, user);

        DateTime? until = null;
        if (parameters?.ContainsKey("Until") == true)
        {
            until = parameters["Until"] as DateTime?;

            if (until == null)
                throw new InvalidDataException();

            // Учитываем текущий часовой пояс
            until = ((DateTime) until).AddHours(3);
            if (until <= DateTime.Now.AddHours(2))
                throw new InvalidDataException();
        }

        var (itemPath, item) = await base.CreateAsync(parentId, name, Type.Task, user);
        var task = new TaskEntity
        {
            Id = item.Guid,
            Until = until
        };
        await _commonTaskQueries.CreateAsync(task);
        return (itemPath, await GetAsync(item.Guid, user, true));
    }

    public async new Task DeleteAsync(string id, User user)
    {
        var path = await base.DeleteAsync(id, user);
        Directory.Delete(path, true);
    }
}

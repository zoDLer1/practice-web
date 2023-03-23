using PracticeWeb.Models;

namespace PracticeWeb.Services.FileSystemServices.Helpers;

public interface IFileSystemHelper
{
    Task<ItemAccess> HasAccessAsync(string id, User user, List<string> path);
    Task CheckIfCanCreateAsync(string parentId, User user);
    Task<object> GetAsync(string id, User user);
    Task<object> GetChildItemAsync(string id, User user);
    Task<(string, object)> CreateAsync(string parentId, string name, User user, Dictionary<string, object>? parameters=null);
    Task<FolderItem> UpdateAsync(string id, string newName, User user);
    Task<FolderItem> UpdateTypeAsync(string id, Type newType, User user);
    Task DeleteAsync(string id, User user);
}

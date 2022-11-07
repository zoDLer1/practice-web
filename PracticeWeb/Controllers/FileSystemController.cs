using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PracticeWeb.Exceptions;
using PracticeWeb.Models;
using PracticeWeb.Services.FileSystemServices;
using PracticeWeb.Services.GroupStorageServices;
using PracticeWeb.Services.ItemStorageServices;

namespace PracticeWeb.Controllers;

[ApiController]
[Route("[controller]")]
public class FileSystemController : ControllerBase
{
    private IFileSystemService _fileSystemService;
    private IGroupStorageService _groupStorageService;
    private IItemStorageService _itemStorageService;

    public FileSystemController(
        IFileSystemService fileSystemService, 
        IGroupStorageService groupStorageService,
        IItemStorageService itemStorageService)
    {
        _fileSystemService = fileSystemService;
        _groupStorageService = groupStorageService;
        _itemStorageService = itemStorageService;
    }

    private void CreateDirectory(string path) 
    {
        Directory.CreateDirectory(path);
    }

    private string PreparePath(string? path)
    {
        if (path == null)
            throw new NullReferenceException();
        
        if (path.StartsWith("/")) 
            path = path.TrimStart('/');
        if (path.EndsWith("/")) 
            path = path.TrimEnd('/');
        return path;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(string? id)
    {
        if (id == null)
            return BadRequest();

        try
        {
            return new JsonResult(await _fileSystemService.GetFolderInfoAsync(id));
        }
        catch (ItemTypeException)
        {
            try {
                return await _fileSystemService.GetFileAsync(id);
            }
            catch (ItemNotFoundException)
            {
                return NotFound();
            }
            catch (ItemTypeException)
            {
                return BadRequest();
            }
        }
    }

    [HttpPost]
    public async Task<IActionResult> UploadFileAsync(string? parentId, IFormFile uploadedFile)
    {
        if (parentId == null || uploadedFile == null)
            return BadRequest();

        try
        {
            await _fileSystemService.CreateFileAsync(parentId, uploadedFile);
        }
        catch (ItemNotFoundException)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateFolderAsync(string? parentId, string? name)
    {
        if (parentId == null || name == null)
            return BadRequest();

        try
        {
            await _fileSystemService.CreateFolderAsync(parentId, name);
        }
        catch (ItemNotFoundException)
        {
            return NotFound();
        }
        return Ok();
    }

    [HttpPatch]
    public async Task<IActionResult> RenameAsync(string? id, string? name)
    {
        if (id == null || name == null)
            return BadRequest();

        await _fileSystemService.RenameAsync(id, name);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(string? id)
    {
        if (id == null)
            return BadRequest();

        try
        {
            await _fileSystemService.RemoveFolder(id);
        }
        catch (ItemTypeException)
        {
            try
            {
                await _fileSystemService.RemoveFileAsync(id);
            }
            catch (PracticeWeb.Exceptions.FileNotFoundException)
            {
                return NotFound();
            }
        }
        catch (FolderNotFoundException)
        {
            return NotFound();
        }

        return Ok();
    }
}

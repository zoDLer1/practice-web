using PracticeWeb.Models;

namespace PracticeWeb;

public class Folder
{
    public string Name { get; set; } = null!;
    public ItemType Type { get; set; } = null!;
    public List<Object> Path { get; set; } = null!;
    public string Guid { get; set; } = null!;

    public IEnumerable<Object> Children { get; set; } = null!;

    public DateTime CreationTime { get; set; }
    public string CreatorName { get; set; } = null!;
}
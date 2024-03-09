namespace CloudStorage.Domain.Entities;

public class Folder
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    /*
    public long UserId { get; set; }
    public User User { get; set; }
    */

    public Guid? FolderId { get; set; }
    public List<Folder>? SubFolders { get; set; }

    public List<File?> Files { get; set; }
}
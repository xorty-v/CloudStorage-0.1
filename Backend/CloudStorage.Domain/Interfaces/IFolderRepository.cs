using CloudStorage.Domain.Entities;

namespace CloudStorage.Domain.Interfaces;

public interface IFolderRepository
{
    public Task<Folder> AddAsync(Folder folder);
    public Task<Folder?> GetByIdAsync(Guid folderId);
}
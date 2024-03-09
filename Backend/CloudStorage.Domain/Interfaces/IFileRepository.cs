using File = CloudStorage.Domain.Entities.File;

namespace CloudStorage.Domain.Interfaces;

public interface IFileRepository
{
    public Task AddAsync(File folder);
    public Task<File?> GetById(Guid fileId, long userId);
}
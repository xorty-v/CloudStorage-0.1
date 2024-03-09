using CloudStorage.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using File = CloudStorage.Domain.Entities.File;

namespace CloudStorage.Persistence.Repositories;

public class FileRepository : IFileRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FileRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(File folder)
    {
        await _dbContext.AddAsync(folder);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<File?> GetById(Guid fileId, long userId)
    {
        return await _dbContext.Files.FirstOrDefaultAsync(f => f.Id == fileId);

        // return await _dbContext.Files.FirstOrDefaultAsync(f => f.Id == fileId && f.Folder.UserId == userId);
    }
}
using CloudStorage.Domain.Entities;
using CloudStorage.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.Persistence.Repositories;

public class FolderRepository : IFolderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FolderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Folder> AddAsync(Folder folder)
    {
        await _dbContext.Folders.AddAsync(folder);
        await _dbContext.SaveChangesAsync();

        return folder;
    }

    public async Task<Folder?> GetByIdAsync(Guid folderId)
    {
        return await _dbContext.Folders.FirstOrDefaultAsync(f => f.Id == folderId);
    }
}
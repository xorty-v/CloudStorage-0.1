using CloudStorage.Domain.Dto;
using Microsoft.AspNetCore.Http;

namespace CloudStorage.Service.Interfaces;

public interface IStorageService
{
    public Task PutFolderAsync(IFormFile zipFile, Guid parentFolderId);
    public Task PutFilesAsync(IFormFileCollection files, Guid folderId);
    public Task<GetFileDto> GetFileDetailsAsync(Guid fileId, long userId);
    public Task GetFileAsync(Guid fileId, Func<Stream, CancellationToken, Task> fileStreamCallback);
    public Task GetFolderAsync(Guid folderId);
}
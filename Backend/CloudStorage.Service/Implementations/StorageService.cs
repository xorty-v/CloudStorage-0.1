using System.IO.Compression;
using CloudStorage.Domain.Dto;
using CloudStorage.Domain.Entities;
using CloudStorage.Domain.Interfaces;
using CloudStorage.Persistence.Extensions;
using CloudStorage.Persistence.Helpers;
using CloudStorage.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using File = CloudStorage.Domain.Entities.File;

namespace CloudStorage.Service.Implementations;

public class StorageService : IStorageService
{
    private readonly IStorageRepository _storageRepository;
    private readonly IFolderRepository _folderRepository;
    private readonly IFileRepository _fileRepository;

    public StorageService(IStorageRepository storageRepository, IFolderRepository folderRepository,
        IFileRepository fileRepository)
    {
        _storageRepository = storageRepository;
        _folderRepository = folderRepository;
        _fileRepository = fileRepository;
    }

    public async Task PutFolderAsync(IFormFile zipFile, Guid parentFolderId)
    {
        var parentFolder = await _folderRepository.GetByIdAsync(parentFolderId);

        if (parentFolder is null)
        {
            parentFolder = await _folderRepository.AddAsync(new Folder()
            {
                Id = Guid.NewGuid(),
                Name = "Home",
                SubFolders = new List<Folder>(),
                Files = new List<File>()
            });
        }
        else
        {
            parentFolder.SubFolders = new List<Folder>();
        }

        using var zipArchive = new ZipArchive(zipFile.OpenReadStream(), ZipArchiveMode.Read);

        foreach (var entry in zipArchive.Entries)
        {
            string[] parts = entry.FullName.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0 || string.IsNullOrEmpty(parts[0]))
                continue;

            Folder currentFolder = parentFolder;

            for (var i = 0; i < parts.Length - 1; i++)
            {
                string folderName = parts[i];

                var nextFolder = currentFolder.SubFolders.FirstOrDefault(folder => folder.Name == parts[i]);

                if (nextFolder == null)
                {
                    nextFolder = new Folder()
                    {
                        Id = Guid.NewGuid(),
                        Name = folderName,
                        SubFolders = new List<Folder>(),
                        Files = new List<File>()
                    };

                    currentFolder.SubFolders.Add(nextFolder);
                    await _folderRepository.AddAsync(nextFolder);
                }

                currentFolder = nextFolder;
            }

            if (!entry.IsFolder())
            {
                var file = new File()
                {
                    Id = Guid.NewGuid(),
                    Name = entry.Name,
                    Size = entry.Length,
                    UploadDate = DateTime.UtcNow,
                    FolderId = currentFolder.Id
                };

                await _fileRepository.AddAsync(file);
                await _storageRepository.PutFileAsync(entry.Open(), entry.Length, file.Id.ToString());
            }
        }
    }

    public async Task PutFilesAsync(IFormFileCollection files, Guid folderId)
    {
        foreach (var file in files)
        {
            File newFile = new File()
            {
                Id = Guid.NewGuid(),
                Name = file.FileName,
                Size = file.Length,
                UploadDate = DateTime.UtcNow,
                FolderId = folderId
            };

            await _fileRepository.AddAsync(newFile);
            await _storageRepository.PutFileAsync(file.OpenReadStream(), file.Length, newFile.Id.ToString());
        }
    }

    public async Task<GetFileDto> GetFileDetailsAsync(Guid fileId, long userId)
    {
        var file = await _fileRepository.GetById(fileId, userId);

        if (file is null)
            throw new Exception("File was not found");

        var fileDetails = new GetFileDto()
        {
            Name = file.Name,
            Length = file.Size,
            MimeType = MimeTypes.GetMimeType(file.Name)
        };

        return fileDetails;
    }

    public async Task GetFileAsync(Guid fileId, Func<Stream, CancellationToken, Task> fileStreamCallback)
    {
        await _storageRepository.GetFileAsync(fileId.ToString(), fileStreamCallback);
    }

    public Task GetFolderAsync(Guid folderId)
    {
        throw new NotImplementedException();
    }
}
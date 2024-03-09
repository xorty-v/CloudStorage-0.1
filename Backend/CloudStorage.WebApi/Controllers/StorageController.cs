using CloudStorage.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StorageController : ControllerBase
{
    private readonly IStorageService _storageService;

    public StorageController(IStorageService storageService)
    {
        _storageService = storageService;
    }

    [HttpGet("getFile")]
    public async Task DownloadFile(Guid fileId)
    {
        var userId = 0; // пока аунтификации нет
        var fileDetails = await _storageService.GetFileDetailsAsync(fileId, userId);

        Response.ContentLength = fileDetails.Length;
        Response.ContentType = fileDetails.MimeType;
        Response.Headers.Add("Content-Disposition", $"attachment; filename={fileDetails.Name}");

        async Task StreamFileAction(Stream stream, CancellationToken ct)
        {
            await stream.CopyToAsync(Response.BodyWriter.AsStream(), ct);
        }

        await _storageService.GetFileAsync(fileId, StreamFileAction);
    }

    [HttpPost("uploadFiles")]
    public async Task<IActionResult> UploadFiles(IFormFileCollection files, Guid folderId)
    {
        await _storageService.PutFilesAsync(files, folderId);

        return Ok();
    }

    [HttpPost("uploadFolder")]
    public async Task<IActionResult> UploadFolder(IFormFile zipFolder, Guid parentFolderId)
    {
        await _storageService.PutFolderAsync(zipFolder, parentFolderId);

        return Ok();
    }
}
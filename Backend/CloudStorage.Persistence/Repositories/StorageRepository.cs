using CloudStorage.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace CloudStorage.Persistence.Repositories;

public class StorageRepository : IStorageRepository
{
    private readonly IMinioClient _minioClient;
    private readonly IConfiguration _configuration;

    public StorageRepository(IMinioClient minioClient, IConfiguration configuration)
    {
        _minioClient = minioClient;
        _configuration = configuration;
    }

    public async Task PutFileAsync(Stream streamFile, long length, string objectName)
    {
        var args = new PutObjectArgs()
            .WithBucket(_configuration["MinIO:Bucket"])
            .WithObject(objectName)
            .WithStreamData(streamFile)
            .WithObjectSize(length);

        await _minioClient.PutObjectAsync(args).ConfigureAwait(false);
    }

    public async Task GetFileAsync(string objectName, Func<Stream, CancellationToken, Task> fileStreamCallback)
    {
        var args = new GetObjectArgs()
            .WithBucket(_configuration["MinIO:Bucket"])
            .WithObject(objectName)
            .WithCallbackStream(fileStreamCallback);

        await _minioClient.GetObjectAsync(args).ConfigureAwait(false);
    }
}
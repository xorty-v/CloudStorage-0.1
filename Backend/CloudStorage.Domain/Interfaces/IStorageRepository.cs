namespace CloudStorage.Domain.Interfaces;

public interface IStorageRepository
{
    public Task PutFileAsync(Stream streamFile, long length, string objectName);
    public Task GetFileAsync(string objectName, Func<Stream, CancellationToken, Task> fileStreamCallback);
}
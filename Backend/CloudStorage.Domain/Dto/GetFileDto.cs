namespace CloudStorage.Domain.Dto;

public class GetFileDto
{
    public string Name { get; set; }
    public long Length { get; set; }
    public string MimeType { get; set; }
}
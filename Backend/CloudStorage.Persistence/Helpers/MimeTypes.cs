namespace CloudStorage.Persistence.Helpers;

public static class MimeTypes
{
    private static readonly Dictionary<string, string> _extensionMapping = new()
    {
        { ".txt", "text/plain" },
        { ".jpg", "image/jpeg" },
        { ".png", "image/png" },
        { ".pdf", "application/pdf" },
        { ".doc", "application/msword" },
        { ".xls", "application/vnd.ms-excel" },
        { ".json", "application/json" },
        { ".xml", "application/xml" },
        { ".zip", "application/zip" },
        { ".rar", "application/x-rar-compressed" },
    };

    public static string GetMimeType(string filePath)
    {
        var extension = Path.GetExtension(filePath);

        if (_extensionMapping.TryGetValue(extension, out var mimeType))
        {
            return mimeType;
        }

        return "application/octet-stream"; // Default to binary if not found
    }
}
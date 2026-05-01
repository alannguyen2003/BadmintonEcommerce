namespace BadmintonEcommerce.Contracts.API.Presentation.File.Request;

public class FileUploadStreamData
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public Stream Stream { get; set; }
}
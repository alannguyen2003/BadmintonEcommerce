namespace SharedKernel.Services;

public class FileUploadStream
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public Stream Stream { get; set; }
}
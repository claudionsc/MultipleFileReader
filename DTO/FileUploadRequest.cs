using Microsoft.AspNetCore.Mvc;

public class FileUploadRequest
{
    [FromForm]
    public IList<IFormFile> Files { get; set; }
}

using Microsoft.AspNetCore.Mvc;
using MultipleFileReader.Interface;

namespace MultipleFileReader.Controllers;

[ApiController]
[Route("[controller]")]
public class MultipleFileReaderController : ControllerBase
{
    private readonly IFileServices _file;
    public MultipleFileReaderController(IFileServices file)
    {
        _file = file;
    }

    [Consumes("multipart/form-data")]
    [HttpPost("upload")]
    public ActionResult UploadFiles([FromForm] List<IFormFile> files)
    {
        if (files == null || !files.Any())
        {
            return BadRequest("No files were uploaded.");
        }

        _file.ReadFileAsync(files);
        
        return Ok("File uploaded successfully.");
    }
}

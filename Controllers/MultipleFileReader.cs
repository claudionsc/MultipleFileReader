using Microsoft.AspNetCore.Mvc;

namespace MultipleFileReader.Controllers;

[ApiController]
[Route("[controller]")]
public class MultipleFileReaderController : ControllerBase
{

    [Consumes("multipart/form-data")]
    [HttpPost("upload")]
    public ActionResult UploadFiles([FromForm] List<IFormFile> files)
    {
        if (files == null || !files.Any())
        {
            return BadRequest("No files were uploaded.");
        }
        
        return Ok("File uploaded successfully.");
    }
}

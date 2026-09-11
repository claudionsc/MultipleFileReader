using Microsoft.AspNetCore.Mvc;
using MultipleFileReader.Interface;
using System.Diagnostics;

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
            return BadRequest("Nenhum arquivo enviado.");
        }

        var sw = Stopwatch.StartNew();
        var fileModels = _file.ReadFile(files);
        sw.Stop();

        Console.WriteLine($"Processamento sequencial (CPU-bound): {sw.ElapsedMilliseconds} ms");

        return Ok(fileModels);
    }

    [Consumes("multipart/form-data")]
    [HttpPost("upload-parallel")]
    public ActionResult UploadFilesParallel([FromForm] List<IFormFile> files)
    {
        if (files == null || !files.Any())
        {
            return BadRequest("Nenhum arquivo enviado.");

        }

        var sw = Stopwatch.StartNew();
        var fileModels = _file.ReadFilesAConcurrent(files);
        sw.Stop();

        Console.WriteLine($"Processamento paralelo (CPU-bound): {sw.ElapsedMilliseconds} ms");

        return Ok(fileModels);
    }
}

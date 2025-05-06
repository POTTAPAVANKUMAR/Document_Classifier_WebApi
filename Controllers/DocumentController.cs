using Microsoft.AspNetCore.Mvc;
using Document_Classifier_WebApi.Service.Interface;
using System.IO;
using Document_Classifier_WebApi;

[ApiController]
[Route("api/[controller]")]
public class DocumentController : ControllerBase
{
    private readonly IProcessedDocRepository _docRepo;

    public DocumentController(IProcessedDocRepository docRepo)
    {
        _docRepo = docRepo;
    }

    [HttpGet("next")]
    public async Task<IActionResult> GetNextDocument()
    {
        var completedDir = AppConst.CompletedDocsPath;
        var files = Directory.GetFiles(completedDir);
        if (files.Length == 0) return NotFound("No documents found.");

        var filePath = files.First();
        var fileName = Path.GetFileName(filePath);
        var doc = await _docRepo.GetByFileNameAsync(fileName);
        if (doc == null) return NotFound("Document not found in DB.");

        var base64Image = Convert.ToBase64String(System.IO.File.ReadAllBytes(filePath));
        return Ok(new
        {
            fileName,
            base64Image,
            ocrText = doc.OcrText,
            aiResult = doc.AiResultJson
        });
    }

    [HttpPost("complete")]
    public IActionResult CompleteDocument([FromBody] string fileName)
    {
        var filePath = Path.Combine(AppConst.CompletedDocsPath, fileName);
        if (!System.IO.File.Exists(filePath)) return NotFound("File not found.");

        System.IO.File.Delete(filePath);
        return Ok("Deleted successfully.");
    }
}
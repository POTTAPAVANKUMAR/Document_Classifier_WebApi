using Document_Classifier_WebApi.Manager.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Document_Classifier_WebApi.Controllers
{
    [ApiController]
    [Route("ocr")]
    public class OcrController : ControllerBase
    {
        private readonly IOcrManager _ocrManager;

        public OcrController(IOcrManager ocrManager)
        {
            _ocrManager = ocrManager;
        }

        [HttpPost]
        public async Task<IActionResult> PerformOcr(IFormFile file)
        {
            if (file == null || !file.ContentType.StartsWith("image/"))
            {
                return BadRequest("Bad request if the file is not provided or is not an image");
            }

            var extractedText = await _ocrManager.PerformOcrAsync(file);
            return Ok(new { text = extractedText });
        }
    }
}

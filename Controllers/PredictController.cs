using Document_Classifier_WebApi.Manager.Interface;
using Document_Classifier_WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace Document_Classifier_WebApi.Controllers
{
    // Controllers/PredictController.cs
    [ApiController]
    [Route("predict")]
    public class PredictController : ControllerBase
    {
        private readonly IPredictManager _predictManager;

        public PredictController(IPredictManager predictManager)
        {
            _predictManager = predictManager;
        }

        [HttpPost]
        public async Task<IActionResult> Predict([FromBody] PredictionRequest request)
        {
            if (request == null || request.Texts == null || request.Texts.Count == 0)
            {
                return BadRequest("Invalid input data");
            }

            var predictions = await _predictManager.PredictAsync(request.Texts);
            return Ok(predictions);
        }
    }
}

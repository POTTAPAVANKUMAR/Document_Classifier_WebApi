using Document_Classifier_WebApi.Manager.Interface;
using Document_Classifier_WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace Document_Classifier_WebApi.Controllers
{
    [ApiController]
    [Route("train")]
    public class TrainController : ControllerBase
    {
        private readonly ITrainManager _trainManager;

        public TrainController(ITrainManager trainManager)
        {
            _trainManager = trainManager;
        }

        [HttpPost]
        public async Task<IActionResult> Train([FromBody] TrainingRequest request)
        {
            if (request == null || request.TrainingData == null || request.TrainingData.Count == 0)
            {
                return BadRequest("Invalid input data");
            }

            var message = await _trainManager.TrainAsync(request.TrainingData);
            return Ok(new { message });
        }
    }
}

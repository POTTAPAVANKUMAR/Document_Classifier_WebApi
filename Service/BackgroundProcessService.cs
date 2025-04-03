using Document_Classifier_WebApi.Manager.Interface;
using Document_Classifier_WebApi.Model;
using Document_Classifier_WebApi.Service.Interface;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Document_Classifier_WebApi.Service
{
    public class BackgroundProcessService : IBackgroundProcessService
    {
        private readonly IOcrManager _ocrManager;
        private readonly IPredictManager _predictManager;
        private readonly string _folderPath;

        public BackgroundProcessService(IOcrManager ocrManager, IPredictManager predictManager, string folderPath = AppConst.NewDocsPath)
        {
            _ocrManager = ocrManager;
            _predictManager = predictManager;
            _folderPath = folderPath;
        }

        public async Task ExecuteAsync()
        {
            var files = Directory.GetFiles(_folderPath);
            foreach (var file in files)
            {
                string text = string.Empty;
                // Convert file to IFormFile
                var fileInfo = new FileInfo(file);
                if (fileInfo.Exists)
                {
                    using (var stream = new FileStream(file, FileMode.Open))
                    {
                        var formFile = new FormFile(stream, 0, stream.Length, fileInfo.Name, fileInfo.Name)
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "application/octet-stream"
                        };
                        // Perform OCR
                        text = await _ocrManager.PerformOcrAsync(formFile);
                    }

                    if (text != null)
                    {
                        // Call predict method
                        List<PredictionResponse> response = await _predictManager.PredictAsync(new List<string> { text });
                    }

                    // Move the file to the completed folder
                    var completedFilePath = Path.Combine(AppConst.CompletedDocsPath, fileInfo.Name);
                    if (!Directory.Exists(AppConst.CompletedDocsPath))
                    {
                        Directory.CreateDirectory(AppConst.CompletedDocsPath);
                    }
                    if (File.Exists(completedFilePath))
                    {
                        File.Delete(completedFilePath);
                    }
                    File.Move(file, completedFilePath);
                }
            }
        }
    }
}

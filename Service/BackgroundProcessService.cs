using Document_Classifier_WebApi.Manager.Interface;
using Document_Classifier_WebApi.Model;
using Document_Classifier_WebApi.Service.Interface;
using Document_Classifier_WebApi.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Document_Classifier_WebApi.Service
{
    public class BackgroundProcessService : IBackgroundProcessService
    {
        private readonly IOcrManager _ocrManager;
        private readonly IOpenAiService _openAiService;
        private readonly IProcessedDocRepository _processedDocRepository;
        private readonly string _folderPath;

        public BackgroundProcessService(
            IOcrManager ocrManager,
            IOpenAiService openAiService,
            IProcessedDocRepository processedDocRepository,
            string folderPath = AppConst.NewDocsPath)
        {
            _ocrManager = ocrManager;
            _openAiService = openAiService;
            _processedDocRepository = processedDocRepository;
            _folderPath = folderPath;
        }

        public async Task ExecuteAsync()
        {
            var files = Directory.GetFiles(_folderPath);
            foreach (var file in files)
            {
                string ocrText = string.Empty;
                var fileInfo = new FileInfo(file);

                if (!fileInfo.Exists)
                    continue;

                using (var stream = new FileStream(file, FileMode.Open))
                {
                    var formFile = new FormFile(stream, 0, stream.Length, fileInfo.Name, fileInfo.Name)
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = "application/octet-stream"
                    };

                    // Step 1: OCR
                    ocrText = await _ocrManager.PerformOcrAsync(formFile);
                }

                if (!string.IsNullOrWhiteSpace(ocrText))
                {
                    // Step 2: Call OpenAI Service
                    var aiResult = await _openAiService.AnalyzeDocumentAsync(ocrText);

                    // Step 3: Save to DB
                    var processed = new ProcessedDocument
                    {
                        FileName = fileInfo.Name,
                        OcrText = ocrText,
                        AiResultJson = JsonSerializer.Serialize(aiResult),
                        ProcessedAt = System.DateTime.UtcNow
                    };

                    await _processedDocRepository.SaveAsync(processed);
                }

                // Step 4: Move file to completed
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

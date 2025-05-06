using System.Threading.Tasks;
using Document_Classifier_WebApi.Model;
using Document_Classifier_WebApi.Service.Interface;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Text.Json;

public class ProcessedDocRepository : IProcessedDocRepository
{
    private readonly string _connectionString;

    public ProcessedDocRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("PostgresDb");
    }

    public async Task SaveAsync(ProcessedDocument doc)
    {
        try
        {
            const string query = @"
            INSERT INTO processed_documents (file_name, ocr_text, ai_result_json, processed_at)
            VALUES (@file_name, @ocr_text, @ai_result_json, @processed_at)";

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("file_name", doc.FileName);
            cmd.Parameters.AddWithValue("ocr_text", doc.OcrText ?? string.Empty);
            string aiResultJson = doc.AiResultJson ?? "{}";

            JsonDocument.Parse(aiResultJson);

            cmd.Parameters.AddWithValue("ai_result_json", NpgsqlTypes.NpgsqlDbType.Jsonb, aiResultJson);

            cmd.Parameters.AddWithValue("processed_at", doc.ProcessedAt);

            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
    
    public async Task<ProcessedDocument> GetByFileNameAsync(string fileName)
    {
        const string query = @"
        SELECT file_name, ocr_text, ai_result_json, processed_at
        FROM processed_documents
        WHERE file_name = @file_name
        LIMIT 1";

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(query, conn);
        cmd.Parameters.AddWithValue("file_name", fileName);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new ProcessedDocument
            {
                FileName = reader.GetString(0),
                OcrText = reader.GetString(1),
                AiResultJson = reader.GetString(2),
                ProcessedAt = reader.GetDateTime(3)
            };
        }

        return null;
    }

}
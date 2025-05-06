public class ProcessedDocument
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string OcrText { get; set; }
    public string AiResultJson { get; set; }
    public DateTime ProcessedAt { get; set; }
}

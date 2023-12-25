using System.Text.Json.Serialization;

public class PagedResponse<T>
{
    [JsonPropertyName("count")]
    public required long Count { get; set; }

    [JsonPropertyName("rows")]
    public required List<T> Rows { get; set; }
}

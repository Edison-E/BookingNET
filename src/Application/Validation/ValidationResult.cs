namespace BookPro.Application.Validation;

public class ValidationResult
{
    public bool Valid => Message.Count == 0;
    public List<string> Message { get; set; } = new List<string>();

    public string GetMessageAsString() => string.Join(", ", Message);
}

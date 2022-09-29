namespace Domain.Models;

public class Quote 
{
    public string? Author { get; set; }
    public string? QuoteText { get; set; }
    public int CategoryId { get; set; }
    public List<string> QuoteImages { get; set; }
    public int Id { get; set; }

    public Quote()
    {
        QuoteImages = new List<string>();
    }
}

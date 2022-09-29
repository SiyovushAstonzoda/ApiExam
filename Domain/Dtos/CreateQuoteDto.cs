namespace Domain.Dtos;
using Microsoft.AspNetCore.Http;

public class CreateQuoteDto
{
    public int Id { get; set; }
    public string QuoteText { get; set; }
    public string Author { get; set; }
    public List<IFormFile> QuoteImageFiles { get; set; } 
    public int CategoryId { get; set; }
}

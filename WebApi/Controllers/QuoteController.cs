using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using Domain.Models;
using Domain.Dtos;
using Domain.Wrapper;
namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuoteController : ControllerBase
{
    private QuoteService _quoteService;
    public QuoteController()
    {
        _quoteService = new QuoteService();
    }

    [HttpGet("GetAllQuotesByCategory")]
    public Responce<List<Quote>> GetAllQuotesByCategory(int id)
    {
        return _quoteService.GetAllQuotesByCategory(id);
    }

    [HttpGet("GetQuoteWithCategory")]
    public Responce<List<GetQuoteDto>> GetQuoteWithCategory()
    {
        return _quoteService.GetQuoteWithCategory();
    }

    [HttpGet("GetAllQuotes")]
    public Responce<List<Quote>> GetAllQuotes()
    {
        return _quoteService.GetAllQuotes();
    }

    [HttpGet("GetRandomQuote")]
    public Responce<List<Quote>> GetRandomQuote()
    {
        return _quoteService.GetRandomQuote();
    }

    [HttpPost("AddQuote")]
    public int AddQuote(Quote quote)
    {
        return _quoteService.AddQuote(quote);
    }

    [HttpPut("UpdateQuote")]
    public int UpdateQuote(Quote quote)
    {
        return _quoteService.UpdateQuote(quote);
    }
    
    [HttpDelete("DeleteQuote")]
    public int DeleteQuote(int id)
    {
        return _quoteService.DeleteQuote(id);
    }
}

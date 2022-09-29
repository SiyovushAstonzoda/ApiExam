using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.DataContext;
using Infrastructure.ServiceInterfaces;
using Infrastructure.Services;
namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuoteController : ControllerBase
{
    private readonly IQuoteService _quoteService;
    public QuoteController(IQuoteService quoteService)
    {
        _quoteService = quoteService;
    }

    [HttpGet("GetAllQuotesByCategory")]
    public async Task<Responce<List<Quote>>> GetAllQuotesByCategory(int id)
    {
        return await _quoteService.GetAllQuotesByCategory(id);
    }

    [HttpGet("GetQuoteWithCategory")]
    public async Task<Responce<List<QuoteCategoryDto>>> GetQuoteWithCategory()
    {
        return await _quoteService.GetQuoteWithCategory();
    }

    [HttpGet("GetAllQuotes")]
    public async Task<Responce<List<Quote>>> GetAllQuotes()
    {
        return await _quoteService.GetAllQuotes();
    }

    [HttpGet("GetRandomQuote")]
    public async Task<Responce<List<Quote>>> GetRandomQuote()
    {
        return await _quoteService.GetRandomQuote();
    }

    [HttpPost("AddQuote")]
    public async Task<Responce<GetQuoteDto>> AddQuote([FromForm]CreateQuoteDto quote)
    {
        return await _quoteService.AddQuote(quote);
    }

    [HttpPut("UpdateQuote")]
    public async Task<Responce<Quote>> UpdateQuote(Quote quote)
    {
        return await _quoteService.UpdateQuote(quote);
    }
    
    [HttpDelete("DeleteQuote")]
    public async Task<Responce<string>> DeleteQuote(int id)
    {
        return await _quoteService.DeleteQuote(id);
    }
}

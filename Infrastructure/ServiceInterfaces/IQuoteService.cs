namespace Infrastructure.ServiceInterfaces;using Domain.Wrapper;
using Domain.Dtos;
using Domain.Models;

public interface IQuoteService
{
    Task<Responce<GetQuoteDto>> AddQuote(CreateQuoteDto quote);
    Task<Responce<Quote>> UpdateQuote(Quote quote);
    Task<Responce<string>> DeleteQuote(int id);
    Task<Responce<List<Quote>>> GetAllQuotesByCategory(int id);
    Task<Responce<List<QuoteCategoryDto>>> GetQuoteWithCategory();
    Task<Responce<List<Quote>>> GetAllQuotes();
    Task<Responce<List<Quote>>> GetRandomQuote();
}

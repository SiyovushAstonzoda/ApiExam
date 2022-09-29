namespace Infrastructure.Services;
using Dapper;
using Domain.Models;
using Domain.Dtos;
using Domain.Wrapper;
using Npgsql;
using Infrastructure.ServiceInterfaces;
using Infrastructure.DataContext;
using Microsoft.AspNetCore.Hosting;
using Npgsql.Internal.TypeHandlers.GeometricHandlers;

public class QuoteService : IQuoteService
{
    private DataContext _context;
    private readonly IWebHostEnvironment _environment;

     public QuoteService(DataContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

     public async Task<Responce<GetQuoteDto>> AddQuote(CreateQuoteDto quote)
    {
        try
        {
            using var connection = _context.CreateConnection();
        {
            var path  = Path.Combine(_environment.WebRootPath, "images", "posts", quote.QuoteImageFile.FileName);
            using var stream = new FileStream(path, FileMode.Create);
            await quote.QuoteImageFile.CopyToAsync(stream);

            var sql = $"insert into Quote (Author, QuoteText, QuoteImage, CategoryId) VALUES (@Author, @QuoteText, @QuoteImage, @CategoryId) returning Id";
            var response  = await connection.ExecuteScalarAsync<int>(sql, new{quote.Author, quote.QuoteText, QuoteImage = quote.QuoteImageFile.FileName, quote.CategoryId});
            quote.Id = response;
            var getQuote = new GetQuoteDto
            {
                Id = quote.Id,
                Author = quote.Author,
                QuoteText = quote.QuoteText,
                QuoteImage = quote.QuoteImageFile.FileName,
                CategoryId = quote.CategoryId
            };
            return new Responce<GetQuoteDto>(getQuote);
        }
        }
        catch (Exception e)
        {
            
          return new Responce<GetQuoteDto>(System.Net.HttpStatusCode.InternalServerError, e.Message);
        }   
    }

    public async Task<Responce<Quote>> UpdateQuote(Quote quote)
    {
        try
        {
            using var connection = _context.CreateConnection();
        {
            string sql = $"update Quote set Author = @Author, QuoteText = @QuoteText, CategoryId = @CategoryId where Id = @Id returning Id";
            var response  = await connection.ExecuteScalarAsync<int>(sql, new{quote.Author, quote.QuoteText, quote.CategoryId, quote.Id});
            quote.Id = response;
            return new Responce<Quote>(quote);
        }
        }
         catch (Exception e)
        {     
           return new Responce<Quote>(System.Net.HttpStatusCode.InternalServerError, e.Message);
        }  
       
    }

     public async Task<Responce<string>> DeleteQuote(int id)
    {
        try
        {
            using var connection = _context.CreateConnection();
        {
            string sql = $"delete from Quote where Id = {id}";
            var response  = await connection.ExecuteScalarAsync<int>(sql);
            id = response;
            return new Responce<string>("Success");
        }
        }
         catch (Exception e)
        {
           return new Responce<string>(System.Net.HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<List<Quote>>> GetAllQuotesByCategory(int id)
    {
        using var connection = _context.CreateConnection();
        {
            var sql = $"select * from Quote where Id = {id};";
            var quotes = await connection.QueryAsync<Quote>(sql);
            return new Responce<List<Quote>>(quotes.ToList());
        }
    }

    public async Task<Responce<List<QuoteCategoryDto>>> GetQuoteWithCategory()
    {
        using var connection = _context.CreateConnection();
        {
            var sql = "select q.Id, q.Author, q.QuoteText, c.Name from Quote as q left join Category as c on c.Id = q.CategoryId;";
            var responce = await connection.QueryAsync<QuoteCategoryDto>(sql);
            return new Responce<List<QuoteCategoryDto>>(responce.ToList());
        }
    }

     public async Task<Responce<List<Quote>>> GetAllQuotes()
    {
        using var connection = _context.CreateConnection();
        {
            var sql = "Select * From Quote";
            var quotes = await connection.QueryAsync<Quote>(sql);
            return new Responce<List<Quote>>(quotes.ToList());
        }
    }

     public async Task<Responce<List<Quote>>> GetRandomQuote()
    {
        using var connection = _context.CreateConnection();
        {
            var sql = "select * from Quote order by random() limit 1";
            var quotes = await connection.QueryAsync<Quote>(sql);
            return new Responce<List<Quote>>(quotes.ToList());
        }
    }

}

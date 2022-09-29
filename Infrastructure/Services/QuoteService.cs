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
using Microsoft.AspNetCore.Http;

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

            var sql = $"insert into Quote (Author, QuoteText, CategoryId) VALUES (@Author, @QuoteText, @CategoryId) returning Id";
            var response  = await connection.ExecuteScalarAsync<int>(sql, new{quote.Author, quote.QuoteText, quote.CategoryId});
            quote.Id = response;

            await InsertImage(quote.QuoteImageFiles, quote.Id);

            var getQuote = new GetQuoteDto
            {
                Id = quote.Id,
                Author = quote.Author,
                QuoteText = quote.QuoteText,
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

    private async Task InsertImage(List<IFormFile> images, int quoteId)
    {
        foreach (var image in images)
        {
            var path  = Path.Combine(_environment.WebRootPath, "images", "posts", image.FileName);
            using var stream = new FileStream(path, FileMode.Create);
            await image.CopyToAsync(stream);

            using var connection = _context.CreateConnection();
            var sql = "insert into QuoteImages (QuoteId, ImageName) values (@QuoteId, @ImageName);";
            await connection.ExecuteAsync(sql, new{QuoteId = quoteId, ImageName = image.FileName});
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
            foreach (var quote in quotes)
            {
                var images = await GetImages(quote.Id);
                quote.QuoteImages = images;
            }
            return new Responce<List<Quote>>(quotes.ToList());
        }
    }

     private async Task<List<string>> GetImages(int quoteId)
    {
        await using var connection = _context.CreateConnection();
        var sql = "select q.imagename  from quoteimages q  where quoteid = @quoteId";
        var result = await connection.QueryAsync<string>(sql, new {quoteId});
        return result.ToList();
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

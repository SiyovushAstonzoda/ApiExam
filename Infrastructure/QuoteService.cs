namespace Infrastructure;
using Dapper;
using Domain.Models;
using Domain.Dtos;
using Domain.Wrapper;
using Npgsql;

public class QuoteService
{
    private string _connectionString;

    public QuoteService()
    {
        _connectionString = "Server=127.0.0.1;Port=5432;Database=ExamApiDb;User Id=postgres;Password=masik00787737";
    }

     public Responce<Quote> AddQuote(Quote quote)
    {
        try
        {
             using (NpgsqlConnection connection  = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            string sql = $"insert into Quote (Author, QuoteText, CategoryId) VALUES (@Author, @QuoteText, @CategoryId) returning Id";
            var response  = connection.ExecuteScalar<int>(sql, new{quote.Author, quote.QuoteText, quote.CategoryId});
            quote.Id = response;
            return new Responce<Quote>(quote);
        }
        }
        catch (Exception e)
        {
            
          return new Responce<Quote>(System.Net.HttpStatusCode.InternalServerError, e.Message);
        }   
    }

    public Responce<Quote> UpdateQuote(Quote quote)
    {
        try
        {
             using(NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            string sql = $"update Quote set Author = @Author, QuoteText = @QuoteText, CategoryId = @CategoryId where Id = @Id returning Id";
            var response  = connection.ExecuteScalar<int>(sql, new{quote.Author, quote.QuoteText, quote.CategoryId, quote.Id});
            quote.Id = response;
            return new Responce<Quote>(quote);
        }
        }
         catch (Exception e)
        {     
           return new Responce<Quote>(System.Net.HttpStatusCode.InternalServerError, e.Message);
        }  
       
    }

     public Responce<string> DeleteQuote(int id)
    {
        try
        {
             using(NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            string sql = $"delete from Quote where Id = {id}";
            var response  = connection.ExecuteScalar<int>(sql);
            id = response;
            return new Responce<string>("Success");
        }
        }
         catch (Exception e)
        {
           return new Responce<string>(System.Net.HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public Responce<List<Quote>> GetAllQuotesByCategory(int id)
    {
       using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            var quotes = connection.Query<Quote>($"select * from Quote where Id = {id};").ToList();
            return new Responce<List<Quote>>(quotes);
        }
    }

    public Responce<List<GetQuoteDto>> GetQuoteWithCategory()
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            var sql = "select q.Id, q.Author, q.QuoteText, c.Name from Quote as q left join Category as c on c.Id = q.CategoryId;";
            var responce = connection.Query<GetQuoteDto>(sql);
            return new Responce<List<GetQuoteDto>>(responce.ToList());
        }
    }

     public Responce<List<Quote>> GetAllQuotes()
    {
        using(NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            var quotes = connection.Query<Quote>("Select * From Quote").ToList();
            return new Responce<List<Quote>>(quotes);
        }
    }

     public Responce<List<Quote>> GetRandomQuote()
    {
        using(NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            var quotes = connection.Query<Quote>("select * from Quote order by random() limit 1").ToList();
            return new Responce<List<Quote>>(quotes.ToList());
        }
    }
}

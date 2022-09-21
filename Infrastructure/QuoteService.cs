namespace Infrastructure;
using Dapper;
using Domain.Models;
using Domain.Dtos;
using Npgsql;

public class QuoteService
{
    private string _connectionString;

    public QuoteService()
    {
        _connectionString = "Server=127.0.0.1;Port=5432;Database=ExamApiDb;User Id=postgres;Password=masik00787737";
    }

     public int AddQuote(Quote quote)
    {
        try
        {
             using (NpgsqlConnection connection  = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            string sql = $"insert into Quote (Author, QuoteText, CategoryId) VALUES ('{quote.Author}', '{quote.QuoteText}', {quote.CategoryId})";
            var response  = connection.Execute(sql);
            return response;
        }
        }
        catch (Exception e)
        {
            
            System.Console.WriteLine(e.Message);
            return 0;
        }   
    }

    public int UpdateQuote(Quote quote)
    {
        try
        {
             using(NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            string sql = $"update Quote set Author = '{quote.Author}', QuoteText = '{quote.QuoteText}', CategoryId = {quote.CategoryId} where Id = {quote.Id}";
            var responce = connection.Execute(sql);
            return responce;
        }
        }
         catch (Exception e)
        {
            
            System.Console.WriteLine(e.Message);
            return 0;
        }  
       
    }

     public int DeleteQuote(int id)
    {
        try
        {
             using(NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            string sql = $"delete from Quote where Id = {id}";
            var responce = connection.Execute(sql);
            return responce;
        }
        }
         catch (Exception e)
        {
            
            System.Console.WriteLine(e.Message);
            return 0;
        }     
    }

    public List<Quote> GetAllQuotesByCategory(int id)
    {
       using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            var quotes = connection.Query<Quote>($"select * from Quote where Id = {id};").ToList();
            return quotes;
        }
    }

    public List<GetQuoteDto> GetQuoteWithCategory()
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            var sql = "select q.Id, q.Author, q.QuoteText, c.Name from Quote as q join Category as c on c.Id = q.CategoryId;";
            var responce = connection.Query<GetQuoteDto>(sql);
            return responce.ToList();
        }
    }

     public List<Quote> GetAllQuotes()
    {
        using(NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            var quotes = connection.Query<Quote>("Select * From Quote").ToList();
            return quotes;
        }
    }

     public List<Quote> GetRandomQuote()
    {
        using(NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            var quotes = connection.Query<Quote>("select * from Quote order by random() limit 1").ToList();
            return quotes;
        }
    }
}

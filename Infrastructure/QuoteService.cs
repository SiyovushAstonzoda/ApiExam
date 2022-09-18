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
        using (NpgsqlConnection connection  = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            string sql = $"insert into Quote (Author, QuoteText) VALUES ('{quote.Author}', '{quote.QuoteText}')";
            var response  = connection.Execute(sql);
            return response;
        }
    }

    public int UpdateQuote(Quote quote)
    {
        using(NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            string sql = $"update Quote set Author = '{quote.Author}', QuoteText = '{quote.QuoteText}' where Id = {quote.Id}";
            var responce = connection.Execute(sql);
            return responce;
        }
    }

     public int DeleteQuote(int id)
    {
        using(NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            string sql = $"delete from Quote where Id = {id}";
            var responce = connection.Execute(sql);
            return responce;
        }
    }

    public List<GetQuoteDto> GetAllQuotesByCategory()
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            string sql = "select q.Id, q.Author, q.QuoteText, q.CategoryId, c.Name from Quote as q join Category as c on q.CategoryId = c.Id;";
            var quotes = connection.Query<GetQuoteDto>(sql).ToList();
            return quotes;
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

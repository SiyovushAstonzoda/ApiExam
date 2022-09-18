namespace Infrastructure;
using Dapper;
using Domain.Models;
using Npgsql;

public class CategoryService
{
    private string _connectionString;

    public CategoryService()
    {
        _connectionString = "Server=127.0.0.1;Port=5432;Database=ExamApiDb;User Id=postgres;Password=masik00787737";
    }

     public int AddCategory(Category category)
    {
        using (NpgsqlConnection connection  = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            string sql = $"insert into Category (Name) VALUES ('{category.Name}')";
            var response  = connection.Execute(sql);
            return response;
        }
    }

    public int UpdateCategory(Category category)
    {
        using(NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            string sql = $"update Category set name = '{category.Name}' where Id = {category.Id}";
            var responce = connection.Execute(sql);
            return responce;
        }
    }

     public int DeleteCategory(int id)
    {
        using(NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            string sql = $"delete from Category where Id = {id}";
            var responce = connection.Execute(sql);
            return responce;
        }
    }

     public List<Category> GetCategories()
    {
        using(NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            var categories = connection.Query<Category>("Select * From Category").ToList();
            return categories;
        }
    }
}

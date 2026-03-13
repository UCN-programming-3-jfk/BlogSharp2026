using BCrypt.Net;
using BlogSharp2026.DataAccess.Interfaces;
using BlogSharp2026.DataAccess.Model;
using Dapper;
namespace BlogSharp2026.DataAccess.SqlClient;
public class AuthorDao : BaseDao, IAuthorDao
{
    public AuthorDao(string connectionString) : base(connectionString) {}

    public bool DeleteAuthor(int id)
    {
        try
        {
            var query = "DELETE FROM Author WHERE Id=@Id";
            using var connection = CreateConnection();
            var rowsAffected = connection.Execute(query, new { id });
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {

            throw new Exception ($"Error while trying to delete author with id='{id}'. Error was: '{ex.Message}'", ex);
        }
    }

    public IEnumerable<Author> GetAllAuthors()
    {
        try
        {
            var query = "SELECT * FROM Author";
            using var connection = CreateConnection();
            return connection.Query<Author>(query);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while trying to get all authors. Error was: '{ex.Message}'", ex);
        }
    }

    public Author? GetAuthorById(int id)
    {
        try
        {
            var query = "SELECT * FROM Author WHERE Id=@Id";
            using var connection = CreateConnection();
            return connection.QuerySingle<Author>(query, new { id });
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while trying to get author with id='{id}'. Error was: '{ex.Message}'", ex);
        }
    }

    public int InsertAuthor(Author author)
    {
        try
        {
            // Hash the cleartext password using BCrypt before saving
            author.PasswordHash = BCrypt.Net.BCrypt.HashPassword(author.PasswordHash);

            var query = "INSERT INTO Author (Id, Email, BlogTitle, PasswordHash) VALUES (@Id, @Email, @BlogTitle, @PasswordHash); SELECT @Id;";
            using var connection = CreateConnection();
            var newId = connection.QuerySingle(query, author);
            return newId;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while trying to insert author with id='{author.Id}', email='{author.Email}', blogTitle='{author.BlogTitle}'. Error was: '{ex.Message}'", ex);
        }
    }

    public bool UpdateAuthor(Author author)
    {
        try
        {
            var query = "UPDATE Author SET Email=@Email, BlogTitle=@BlogTitle, PasswordHash=@PasswordHash WHERE Id=@Id";
            using var connection = CreateConnection();
            var rowsAffected = connection.Execute(query, author);
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while trying to update author with id='{author.Id}', email='{author.Email}', blogTitle='{author.BlogTitle}'. Error was: '{ex.Message}'", ex);
        }
    }
}

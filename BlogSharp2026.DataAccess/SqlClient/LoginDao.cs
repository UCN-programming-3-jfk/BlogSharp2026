using BCrypt.Net;
using BlogSharp2026.DataAccess.Interfaces;
using BlogSharp2026.DataAccess.Model;
using Dapper;

namespace BlogSharp2026.DataAccess.SqlClient;

public class LoginDao : BaseDao, ILoginDataAccess
{
    public LoginDao(string connectionString) : base(connectionString) { }

    public Author? GetAuthorByEmailAndPassword(LoginInfo loginInfo)
    {
        try
        {
            // First, retrieve the author by email to get the stored password hash
            var query = "SELECT * FROM Author WHERE Email=@Email";
            using var connection = CreateConnection();
            var author = connection.QuerySingleOrDefault<Author>(query, new { loginInfo.Email });

            if (author == null)
            {
                return null;
            }

            // Verify the provided password against the stored BCrypt hash
            // BCrypt.Verify handles salt extraction from the stored hash automatically
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginInfo.Password, author.PasswordHash);

            if (isPasswordValid)
            {
                return author;
            }

            return null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while trying to authenticate author with email='{loginInfo.Email}'. Error was: '{ex.Message}'", ex);
        }
    }
}

using BlogSharp2026.DataAccess.Interfaces;
using BlogSharp2026.DataAccess.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSharp2026.DataAccess.SqlClient;

public class AuthorDao : BaseDao, IAuthorDao
{
    public AuthorDao(string connectionString) : base(connectionString)
    {
        
    }

    public bool DeleteAuthor(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Author> GetAllAuthors()
    {
        throw new NotImplementedException();
    }

    public Author GetAuthorById(int id)
    {
        var query = "SELECT * FROM Author WHERE Id=@Id";
        using var connection = CreateConnection();
        return connection.QuerySingle<Author>(query, new { id });

    }

    public bool InsertAuthor(Author author)
    {
        throw new NotImplementedException();
    }

    public bool UpdateAuthor(Author author)
    {
        throw new NotImplementedException();
    }
}

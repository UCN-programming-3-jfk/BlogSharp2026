using BlogSharp2026.DataAccess.Model;

namespace BlogSharp2026.DataAccess.Interfaces;

public interface IAuthorDao
{
    Author? GetAuthorById(int id);
    IEnumerable<Author> GetAllAuthors();
    int InsertAuthor(Author author);
    bool UpdateAuthor(Author author);
    bool DeleteAuthor(int id);
}
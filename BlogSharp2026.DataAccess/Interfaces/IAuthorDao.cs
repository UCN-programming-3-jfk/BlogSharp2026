using BlogSharp2026.DataAccess.Model;

namespace BlogSharp2026.DataAccess.Interfaces;

public interface IAuthorDao
{
    IEnumerable<Author> GetAllAuthors();
    Author GetAuthorById(int id);
    bool UpdateAuthor(Author author);
    bool DeleteAuthor(int id);
    bool InsertAuthor(Author author);

}

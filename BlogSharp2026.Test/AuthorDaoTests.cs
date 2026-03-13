using BlogSharp2026.DataAccess.Model;
using BlogSharp2026.DataAccess.SqlClient;

namespace BlogSharp2026.Test;

public class AuthorDaoTests
{
    const string CONNECTION_STRING = "Data Source =.;Initial Catalog=BlogSharp2026; Integrated Security = True;Trust Server Certificate = True;";

    AuthorDao _authorDao;
    List<int> _cleanupAuthorIds = new List<int>();

    [SetUp]
    public void Setup()
    {
        _authorDao = new AuthorDao(CONNECTION_STRING);
    }

    [TearDown]
    public void CleanUp()
    {
        // Clean up test authors
        foreach (var id in _cleanupAuthorIds)
        {
            try
            {
                _authorDao.DeleteAuthor(id);
            }
            catch { }
        }
        _cleanupAuthorIds.Clear();
    }

    [Test]
    public void GetAuthorById_ShouldReturnAuthor_WhenAuthorWithThatIdExists()
    {
        // Arrange
        var testAuthor = CreateTestAuthor();

        // Act
        Author? author = _authorDao.GetAuthorById(testAuthor.Id);

        // Assert
        Assert.That(author, Is.Not.Null, "Author was not found");
        Assert.That(author.Id, Is.EqualTo(testAuthor.Id));
        Assert.That(author.Email, Is.EqualTo(testAuthor.Email));
    }

    [Test]
    public void GetAllAuthors_ShouldReturnAuthors_WhenAuthorsExist()
    {
        // Arrange
        var testAuthor1 = CreateTestAuthor();
        var testAuthor2 = CreateTestAuthor();

        // Act
        IEnumerable<Author> authors = _authorDao.GetAllAuthors();

        // Assert
        Assert.That(authors, Is.Not.Null, "Authors collection should not be null");
        Assert.That(authors.Count(), Is.GreaterThanOrEqualTo(2), "Should return at least 2 authors");
    }

    [Test]
    public void InsertAuthor_ShouldReturnId_WhenAuthorIsInserted()
    {
        // Arrange
        var author = new Author
        {
            Id = GenerateUniqueId(),
            BlogTitle = $"Test Blog {Guid.NewGuid()}",
            Email = $"test{Guid.NewGuid()}@example.com",
            PasswordHash = "cleartextPassword123" // Cleartext password - will be hashed by InsertAuthor
        };
        _cleanupAuthorIds.Add(author.Id);

        // Act
        int newId = _authorDao.InsertAuthor(author);

        // Assert
        Assert.That(newId, Is.EqualTo(author.Id), "Returned ID should match the inserted author ID");

        // Verify the author was actually inserted
        Author? insertedAuthor = _authorDao.GetAuthorById(newId);
        Assert.That(insertedAuthor, Is.Not.Null, "Inserted author should be retrievable");
        Assert.That(insertedAuthor.Email, Is.EqualTo(author.Email));
        // Verify password was hashed (should start with BCrypt prefix)
        Assert.That(insertedAuthor.PasswordHash, Does.StartWith("$2"), "Password should be hashed with BCrypt");
    }

    [Test]
    public void UpdateAuthor_ShouldReturnTrue_WhenAuthorIsUpdated()
    {
        // Arrange
        var testAuthor = CreateTestAuthor();
        string updatedEmail = $"updated{Guid.NewGuid()}@example.com";
        string updatedBlogTitle = $"Updated Blog {Guid.NewGuid()}";

        testAuthor.Email = updatedEmail;
        testAuthor.BlogTitle = updatedBlogTitle;

        // Act
        bool result = _authorDao.UpdateAuthor(testAuthor);

        // Assert
        Assert.That(result, Is.True, "Update should return true");

        // Verify the author was actually updated
        Author? updatedAuthor = _authorDao.GetAuthorById(testAuthor.Id);
        Assert.That(updatedAuthor, Is.Not.Null, "Updated author should be retrievable");
        Assert.That(updatedAuthor.Email, Is.EqualTo(updatedEmail));
        Assert.That(updatedAuthor.BlogTitle, Is.EqualTo(updatedBlogTitle));
    }

    [Test]
    public void UpdateAuthor_ShouldReturnFalse_WhenAuthorDoesNotExist()
    {
        // Arrange
        var author = new Author
        {
            Id = 999999,
            BlogTitle = "Non-existent Blog",
            Email = "nonexistent@example.com",
            PasswordHash = "$2a$11$TestHashTestHashTestHashTestHashTestHashTestHashTestHashTe"
        };

        // Act
        bool result = _authorDao.UpdateAuthor(author);

        // Assert
        Assert.That(result, Is.False, "Update should return false for non-existent author");
    }

    [Test]
    public void DeleteAuthor_ShouldReturnTrue_WhenAuthorIsDeleted()
    {
        // Arrange
        var testAuthor = CreateTestAuthor();
        _cleanupAuthorIds.Remove(testAuthor.Id); // Remove from cleanup since we're deleting it

        // Act
        bool result = _authorDao.DeleteAuthor(testAuthor.Id);

        // Assert
        Assert.That(result, Is.True, "Delete should return true");
    }

    [Test]
    public void DeleteAuthor_ShouldReturnFalse_WhenAuthorDoesNotExist()
    {
        // Arrange
        int nonExistentId = 999999;

        // Act
        bool result = _authorDao.DeleteAuthor(nonExistentId);

        // Assert
        Assert.That(result, Is.False, "Delete should return false for non-existent author");
    }

    #region Helper Methods

    private int GenerateUniqueId()
    {
        return Math.Abs(Guid.NewGuid().GetHashCode());
    }

    private Author CreateTestAuthor()
    {
        var author = new Author
        {
            Id = GenerateUniqueId(),
            BlogTitle = $"Test Blog {Guid.NewGuid()}",
            Email = $"test{Guid.NewGuid()}@example.com",
            PasswordHash = "testPassword123" // Cleartext password - will be hashed by InsertAuthor
        };

        _authorDao.InsertAuthor(author);
        _cleanupAuthorIds.Add(author.Id);

        // Retrieve the author to get the hashed password
        return _authorDao.GetAuthorById(author.Id)!;
    }

    #endregion
}
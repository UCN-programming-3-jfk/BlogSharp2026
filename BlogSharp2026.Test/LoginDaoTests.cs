using BlogSharp2026.DataAccess.Model;
using BlogSharp2026.DataAccess.SqlClient;

namespace BlogSharp2026.Test;

public class LoginDaoTests
{
    
    const string TEST_PASSWORD = "testPassword123";

    LoginDao _loginDao;
    AuthorDao _authorDao;
    List<int> _cleanupAuthorIds = new List<int>();

    [SetUp]
    public void Setup()
    {
        _loginDao = new LoginDao(TestSettings.CONNECTION_STRING);
        _authorDao = new AuthorDao(TestSettings.CONNECTION_STRING);
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
    public void GetAuthorByEmailAndPassword_ShouldReturnAuthor_WhenCredentialsAreValid()
    {
        // Arrange
        var testAuthor = CreateTestAuthor(TEST_PASSWORD);
        var loginInfo = new LoginInfo
        {
            Email = testAuthor.Email,
            Password = TEST_PASSWORD
        };

        // Act
        Author? author = _loginDao.GetAuthorByEmailAndPassword(loginInfo);

        // Assert
        Assert.That(author, Is.Not.Null, "Author should be returned for valid credentials");
        Assert.That(author.Id, Is.EqualTo(testAuthor.Id));
        Assert.That(author.Email, Is.EqualTo(testAuthor.Email));
    }

    [Test]
    public void GetAuthorByEmailAndPassword_ShouldReturnNull_WhenPasswordIsIncorrect()
    {
        // Arrange
        var testAuthor = CreateTestAuthor(TEST_PASSWORD);
        var loginInfo = new LoginInfo
        {
            Email = testAuthor.Email,
            Password = "wrongPassword"
        };

        // Act
        Author? author = _loginDao.GetAuthorByEmailAndPassword(loginInfo);

        // Assert
        Assert.That(author, Is.Null, "Author should not be returned for incorrect password");
    }

    [Test]
    public void GetAuthorByEmailAndPassword_ShouldReturnNull_WhenEmailDoesNotExist()
    {
        // Arrange
        var loginInfo = new LoginInfo
        {
            Email = "nonexistent@example.com",
            Password = TEST_PASSWORD
        };

        // Act
        Author? author = _loginDao.GetAuthorByEmailAndPassword(loginInfo);

        // Assert
        Assert.That(author, Is.Null, "Author should not be returned for non-existent email");
    }

    [Test]
    public void GetAuthorByEmailAndPassword_ShouldReturnNull_WhenEmailIsCorrectButPasswordIsEmpty()
    {
        // Arrange
        var testAuthor = CreateTestAuthor(TEST_PASSWORD);
        var loginInfo = new LoginInfo
        {
            Email = testAuthor.Email,
            Password = ""
        };

        // Act
        Author? author = _loginDao.GetAuthorByEmailAndPassword(loginInfo);

        // Assert
        Assert.That(author, Is.Null, "Author should not be returned for empty password");
    }

    [Test]
    public void GetAuthorByEmailAndPassword_ShouldBeCaseSensitiveForPassword()
    {
        // Arrange
        var testAuthor = CreateTestAuthor("TestPassword123");
        var loginInfo = new LoginInfo
        {
            Email = testAuthor.Email,
            Password = "testpassword123" // Different case
        };

        // Act
        Author? author = _loginDao.GetAuthorByEmailAndPassword(loginInfo);

        // Assert
        Assert.That(author, Is.Null, "Password comparison should be case-sensitive");
    }

    [Test]
    public void GetAuthorByEmailAndPassword_ShouldWorkWithSpecialCharactersInPassword()
    {
        // Arrange
        string specialPassword = "P@$$w0rd!#%^&*()";
        var testAuthor = CreateTestAuthor(specialPassword);
        var loginInfo = new LoginInfo
        {
            Email = testAuthor.Email,
            Password = specialPassword
        };

        // Act
        Author? author = _loginDao.GetAuthorByEmailAndPassword(loginInfo);

        // Assert
        Assert.That(author, Is.Not.Null, "Author should be returned for valid credentials with special characters");
        Assert.That(author.Id, Is.EqualTo(testAuthor.Id));
    }

    #region Helper Methods

    private int GenerateUniqueId()
    {
        return Math.Abs(Guid.NewGuid().GetHashCode());
    }

    private Author CreateTestAuthor(string password)
    {
        var author = new Author
        {
            BlogTitle = $"Test Blog {Guid.NewGuid()}",
            Email = $"test{Guid.NewGuid()}@example.com",
            PasswordHash = password // Cleartext password - will be hashed by InsertAuthor
        };

        var newId =_authorDao.InsertAuthor(author);
        _cleanupAuthorIds.Add(newId);

        // Retrieve the author to get the hashed password
        return _authorDao.GetAuthorById(newId)!;
    }

    #endregion
}

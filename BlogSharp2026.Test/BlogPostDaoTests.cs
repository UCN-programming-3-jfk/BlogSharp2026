using BlogSharp2026.DataAccess.Model;
using BlogSharp2026.DataAccess.SqlClient;

namespace BlogSharp2026.Test;

public class BlogPostDaoTests
{
   

    BlogPostDao _blogPostDao;
    AuthorDao _authorDao;
    List<int> _cleanupBlogPostIds = new List<int>();
    List<int> _cleanupAuthorIds = new List<int>();

    [SetUp]
    public void Setup()
    {
        _blogPostDao = new BlogPostDao(TestSettings.CONNECTION_STRING);
        _authorDao = new AuthorDao(TestSettings.CONNECTION_STRING);
    }

    [TearDown]
    public void CleanUp()
    {
        // Clean up test blog posts
        foreach (var id in _cleanupBlogPostIds)
        {
            try
            {
                _blogPostDao.DeleteBlogPost(id);
            }
            catch { }
        }
        _cleanupBlogPostIds.Clear();

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
    public void GetBlogPostById_ShouldReturnBlogPost_WhenBlogPostWithThatIdExists()
    {
        // Arrange
        var testAuthor = CreateTestAuthor();
        var testBlogPost = CreateTestBlogPost(testAuthor.Id);

        // Act
        BlogPost? blogPost = _blogPostDao.GetBlogPostById(testBlogPost.Id);

        // Assert
        Assert.That(blogPost, Is.Not.Null, "Blog post was not found");
        Assert.That(blogPost.Id, Is.EqualTo(testBlogPost.Id));
        Assert.That(blogPost.PostTitle, Is.EqualTo(testBlogPost.PostTitle));
    }

    [Test]
    public void GetBlogPostById_ShouldReturnNull_WhenBlogPostDoesNotExist()
    {
        // Arrange
        int nonExistentId = 999999;

        // Act
        BlogPost? blogPost = _blogPostDao.GetBlogPostById(nonExistentId);

        // Assert
        Assert.That(blogPost, Is.Null, "Blog post should not be found");
    }

    [Test]
    public void GetAllBlogPosts_ShouldReturnBlogPosts_WhenBlogPostsExist()
    {
        // Arrange
        var testAuthor = CreateTestAuthor();
        var testBlogPost1 = CreateTestBlogPost(testAuthor.Id);
        var testBlogPost2 = CreateTestBlogPost(testAuthor.Id);

        // Act
        IEnumerable<BlogPost> blogPosts = _blogPostDao.GetAllBlogPosts();

        // Assert
        Assert.That(blogPosts, Is.Not.Null, "Blog posts collection should not be null");
        Assert.That(blogPosts.Count(), Is.GreaterThanOrEqualTo(2), "Should return at least 2 blog posts");
    }

    [Test]
    public void GetBlogPostsByAuthorId_ShouldReturnBlogPosts_WhenAuthorHasBlogPosts()
    {
        // Arrange
        var testAuthor = CreateTestAuthor();
        var testBlogPost1 = CreateTestBlogPost(testAuthor.Id);
        var testBlogPost2 = CreateTestBlogPost(testAuthor.Id);

        // Act
        IEnumerable<BlogPost> blogPosts = _blogPostDao.GetBlogPostsByAuthorId(testAuthor.Id);

        // Assert
        Assert.That(blogPosts, Is.Not.Null, "Blog posts collection should not be null");
        Assert.That(blogPosts.Count(), Is.EqualTo(2), "Should return exactly 2 blog posts for this author");
        Assert.That(blogPosts.All(bp => bp.FK_Author_Id == testAuthor.Id), Is.True, "All blog posts should belong to the test author");
    }

    [Test]
    public void GetBlogPostsByAuthorId_ShouldReturnEmptyCollection_WhenAuthorHasNoBlogPosts()
    {
        // Arrange
        var testAuthor = CreateTestAuthor();

        // Act
        IEnumerable<BlogPost> blogPosts = _blogPostDao.GetBlogPostsByAuthorId(testAuthor.Id);

        // Assert
        Assert.That(blogPosts, Is.Not.Null, "Blog posts collection should not be null");
        Assert.That(blogPosts.Count(), Is.EqualTo(0), "Should return no blog posts for this author");
    }

    [Test]
    public void InsertBlogPost_ShouldReturnId_WhenBlogPostIsInserted()
    {
        // Arrange
        var testAuthor = CreateTestAuthor();
        var blogPost = new BlogPost
        {
            FK_Author_Id = testAuthor.Id,
            PostTitle = $"Test Post {Guid.NewGuid()}",
            PostContent = "This is test content for the blog post.",
            CreationDate = DateTime.Now
        };

        // Act
        int newId = _blogPostDao.InsertBlogPost(blogPost);
        _cleanupBlogPostIds.Add(newId);

        // Assert
        Assert.That(newId, Is.GreaterThan(0), "Returned ID should match the inserted blog post ID");

        // Verify the blog post was actually inserted
        BlogPost? insertedBlogPost = _blogPostDao.GetBlogPostById(newId);
        Assert.That(insertedBlogPost, Is.Not.Null, "Inserted blog post should be retrievable");
        Assert.That(insertedBlogPost.PostTitle, Is.EqualTo(blogPost.PostTitle));
    }

    [Test]
    public void UpdateBlogPost_ShouldReturnTrue_WhenBlogPostIsUpdated()
    {
        // Arrange
        var testAuthor = CreateTestAuthor();
        var testBlogPost = CreateTestBlogPost(testAuthor.Id);
        string updatedTitle = $"Updated Title {Guid.NewGuid()}";
        string updatedContent = "This is updated content.";

        testBlogPost.PostTitle = updatedTitle;
        testBlogPost.PostContent = updatedContent;

        // Act
        bool result = _blogPostDao.UpdateBlogPost(testBlogPost);

        // Assert
        Assert.That(result, Is.True, "Update should return true");

        // Verify the blog post was actually updated
        BlogPost? updatedBlogPost = _blogPostDao.GetBlogPostById(testBlogPost.Id);
        Assert.That(updatedBlogPost, Is.Not.Null, "Updated blog post should be retrievable");
        Assert.That(updatedBlogPost.PostTitle, Is.EqualTo(updatedTitle));
        Assert.That(updatedBlogPost.PostContent, Is.EqualTo(updatedContent));
    }

    [Test]
    public void UpdateBlogPost_ShouldReturnFalse_WhenBlogPostDoesNotExist()
    {
        // Arrange
        var blogPost = new BlogPost
        {
            Id = 999999,
            FK_Author_Id = 1,
            PostTitle = "Non-existent Post",
            PostContent = "This post does not exist.",
            CreationDate = DateTime.Now
        };

        // Act
        bool result = _blogPostDao.UpdateBlogPost(blogPost);

        // Assert
        Assert.That(result, Is.False, "Update should return false for non-existent blog post");
    }

    [Test]
    public void DeleteBlogPost_ShouldReturnTrue_WhenBlogPostIsDeleted()
    {
        // Arrange
        var testAuthor = CreateTestAuthor();
        var testBlogPost = CreateTestBlogPost(testAuthor.Id);
        _cleanupBlogPostIds.Remove(testBlogPost.Id); // Remove from cleanup since we're deleting it

        // Act
        bool result = _blogPostDao.DeleteBlogPost(testBlogPost.Id);

        // Assert
        Assert.That(result, Is.True, "Delete should return true");

        // Verify the blog post was actually deleted
        BlogPost? deletedBlogPost = _blogPostDao.GetBlogPostById(testBlogPost.Id);
        Assert.That(deletedBlogPost, Is.Null, "Deleted blog post should not be retrievable");
    }

    [Test]
    public void DeleteBlogPost_ShouldReturnFalse_WhenBlogPostDoesNotExist()
    {
        // Arrange
        int nonExistentId = 999999;

        // Act
        bool result = _blogPostDao.DeleteBlogPost(nonExistentId);

        // Assert
        Assert.That(result, Is.False, "Delete should return false for non-existent blog post");
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
            BlogTitle = $"Test Blog {Guid.NewGuid()}",
            Email = $"test{Guid.NewGuid()}@example.com",
            PasswordHash = "testPassword123" // Cleartext password - will be hashed by InsertAuthor
        };

        var newId =_authorDao.InsertAuthor(author);
        _cleanupAuthorIds.Add(newId);

        // Retrieve the author to get the hashed password
        return _authorDao.GetAuthorById(newId)!;
    }

    private BlogPost CreateTestBlogPost(int authorId)
    {
        var blogPost = new BlogPost
        {
            FK_Author_Id = authorId,
            PostTitle = $"Test Post {Guid.NewGuid()}",
            PostContent = "This is test content for the blog post.",
            CreationDate = DateTime.Now
        };

        var newId =_blogPostDao.InsertBlogPost(blogPost);
        _cleanupBlogPostIds.Add(newId);
        blogPost.Id = newId;

        return blogPost;
    }

    #endregion
}

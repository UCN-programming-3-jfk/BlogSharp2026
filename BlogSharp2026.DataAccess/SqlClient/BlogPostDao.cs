using BlogSharp2026.DataAccess.Interfaces;
using BlogSharp2026.DataAccess.Model;
using Dapper;

namespace BlogSharp2026.DataAccess.SqlClient;

public class BlogPostDao : BaseDao, IBlogPostDao
{
    public BlogPostDao(string connectionString) : base(connectionString) { }

    public bool DeleteBlogPost(int id)
    {
        try
        {
            var query = "DELETE FROM BlogPost WHERE Id=@Id";
            using var connection = CreateConnection();
            var rowsAffected = connection.Execute(query, new { id });
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while trying to delete blog post with id='{id}'. Error was: '{ex.Message}'", ex);
        }
    }

    public IEnumerable<BlogPost> GetAllBlogPosts()
    {
        try
        {
            var query = "SELECT * FROM BlogPost";
            using var connection = CreateConnection();
            return connection.Query<BlogPost>(query);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while trying to get all blog posts. Error was: '{ex.Message}'", ex);
        }
    }

    public BlogPost? GetBlogPostById(int id)
    {
        try
        {
            var query = "SELECT * FROM BlogPost WHERE Id=@Id";
            using var connection = CreateConnection();
            return connection.QuerySingleOrDefault<BlogPost>(query, new { id });
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while trying to get blog post with id='{id}'. Error was: '{ex.Message}'", ex);
        }
    }

    public IEnumerable<BlogPost> GetBlogPostsByAuthorId(int authorId)
    {
        try
        {
            var query = "SELECT * FROM BlogPost WHERE FK_Author_Id=@AuthorId";
            using var connection = CreateConnection();
            return connection.Query<BlogPost>(query, new { AuthorId = authorId });
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while trying to get blog posts for author with id='{authorId}'. Error was: '{ex.Message}'", ex);
        }
    }

    public IEnumerable<BlogPost> GetLatestBlogPosts(int authorId, int numberOfBlogPostsToRetrieve = 10)
    {
        try
        {
            var query = "SELECT TOP(@NumberOfPosts) * FROM BlogPost WHERE FK_Author_Id=@AuthorId ORDER BY CreationDate DESC";
            using var connection = CreateConnection();
            return connection.Query<BlogPost>(query, new { AuthorId = authorId, NumberOfPosts = numberOfBlogPostsToRetrieve });
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while trying to get latest blog posts for author with id='{authorId}'. Error was: '{ex.Message}'", ex);
        }
    }

    public int InsertBlogPost(BlogPost blogPost)
    {
        try
        {
            var query = "INSERT INTO BlogPost (FK_Author_Id, PostTitle, PostContent, CreationDate) OUTPUT Inserted.Id VALUES (@FK_Author_Id, @PostTitle, @PostContent, @CreationDate);";
            using var connection = CreateConnection();
            var newId = connection.QuerySingle<int>(query, blogPost);
            return newId;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while trying to insert blog post: {blogPost}. Error was: '{ex.Message}'", ex);
        }
    }

    public bool UpdateBlogPost(BlogPost blogPost)
    {
        try
        {
            var query = "UPDATE BlogPost SET FK_Author_Id=@FK_Author_Id, PostTitle=@PostTitle, PostContent=@PostContent, CreationDate=@CreationDate WHERE Id=@Id";
            using var connection = CreateConnection();
            var rowsAffected = connection.Execute(query, blogPost);
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while trying to update blog post: {blogPost}. Error was: '{ex.Message}'", ex);
        }
    }
}
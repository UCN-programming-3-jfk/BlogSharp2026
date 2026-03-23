using BlogSharp2026.DataAccess.Model;
namespace BlogSharp2026.DataAccess.Interfaces;
public interface IBlogPostDao
{
    BlogPost? GetBlogPostById(int id);
    IEnumerable<BlogPost> GetAllBlogPosts();
    IEnumerable<BlogPost> GetBlogPostsByAuthorId(int authorId);
    IEnumerable<BlogPost> GetLatestBlogPosts(int numberOfBlogPostsToRetrieve = 10);
    int InsertBlogPost(BlogPost blogPost);
    bool UpdateBlogPost(BlogPost blogPost);
    bool DeleteBlogPost(int id);
}
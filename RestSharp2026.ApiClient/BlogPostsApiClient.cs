using BlogSharp2026.DataAccess.Interfaces;
using BlogSharp2026.DataAccess.Model;
using RestSharp;

namespace RestSharp2026.ApiClient;

public class BlogPostsApiClient : IBlogPostDao
{
    RestClient _client;
    
    public BlogPostsApiClient(string restUrl)
    {
        _client = new RestClient(restUrl);
    }

    public bool DeleteBlogPost(int id)
    {
        var request = new RestRequest($"{id}", Method.Delete);
        var response = _client.Execute(request);
        return response.IsSuccessful;
    }

    public IEnumerable<BlogPost> GetAllBlogPosts()
    {
        var response = _client.Get<IEnumerable<BlogPost>>(new RestRequest());
        return response ?? new List<BlogPost>();
    }

    public BlogPost? GetBlogPostById(int id)
    {
        var request = new RestRequest($"{id}");
        var response = _client.Execute<BlogPost>(request);
        
        if (response.IsSuccessful && response.Data != null)
        {
            return response.Data;
        }
        
        return null;
    }

    public IEnumerable<BlogPost> GetBlogPostsByAuthorId(int authorId)
    {
        // This endpoint is on the AuthorsController, not BlogPostsController
        var request = new RestRequest($"https://localhost:7113/api/v1/authors/{authorId}/blogposts");
        var response = _client.Get<IEnumerable<BlogPost>>(request);
        return response ?? new List<BlogPost>();
    }

    public IEnumerable<BlogPost> GetLatestBlogPosts(int authorId, int numberOfBlogPostsToRetrieve = 10)
    {
        var request = new RestRequest();
        request.AddQueryParameter("numberofblogposts", numberOfBlogPostsToRetrieve);
        var response = _client.Get<IEnumerable<BlogPost>>(request);
        return response ?? new List<BlogPost>();
    }

    public int InsertBlogPost(BlogPost blogPost)
    {
        var request = new RestRequest("", Method.Post);
        request.AddJsonBody(blogPost);
        var response = _client.Execute<int>(request);

        if (response.IsSuccessful && response.Data != 0)
        {
            return response.Data;
        }

        throw new Exception($"Failed to insert blog post. Status: {response.StatusCode}, Error: {response.ErrorMessage}");
    }

    public bool UpdateBlogPost(BlogPost blogPost)
    {
        var request = new RestRequest($"{blogPost.Id}", Method.Put);
        request.AddJsonBody(blogPost);
        var response = _client.Execute(request);
        return response.IsSuccessful;
    }
}

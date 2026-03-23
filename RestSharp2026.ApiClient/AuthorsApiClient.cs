using BlogSharp2026.DataAccess.Interfaces;
using BlogSharp2026.DataAccess.Model;
using RestSharp;

namespace RestSharp2026.ApiClient;

public class AuthorsApiClient : IAuthorDao
{

    RestClient _client;
    //https://localhost:7113/api/v1/authors
    public AuthorsApiClient(string restUrl)
    {
        //instantiates and saves the RestSharp client
        //for use in the CRUD methods
        _client = new RestClient(restUrl);
    }
    public bool DeleteAuthor(int id)
    {
        var request = new RestRequest($"{id}", Method.Delete);
        var response = _client.Execute(request);
        return response.IsSuccessful;
    }

    public IEnumerable<Author> GetAllAuthors()
    {
        return _client.Get<IEnumerable<Author>>(new RestRequest());
    }

    public Author? GetAuthorById(int id)
    {
        var request = new RestRequest($"{id}");
        var response = _client.Execute<Author>(request);

        if (response.IsSuccessful && response.Data != null)
        {
            return response.Data;
        }

        // Return null for 404 or any unsuccessful response
        return null;
    }

    public int InsertAuthor(Author author)
    {
        var request = new RestRequest("", Method.Post);
        request.AddJsonBody(author);
        var response = _client.Execute<int>(request);

        if (response.IsSuccessful && response.Data != 0)
        {
            return response.Data;
        }

        throw new Exception($"Failed to insert author. Status: {response.StatusCode}, Error: {response.ErrorMessage}");
    }

    public bool UpdateAuthor(Author author)
    {
        var request = new RestRequest($"{author.Id}", Method.Put);
        request.AddJsonBody(author);
        var response = _client.Execute(request);
        return response.IsSuccessful;
    }
}

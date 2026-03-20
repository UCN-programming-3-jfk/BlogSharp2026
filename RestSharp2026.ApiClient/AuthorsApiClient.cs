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
        throw new NotImplementedException();
    }

    public IEnumerable<Author> GetAllAuthors()
    {
        return _client.Get<IEnumerable<Author>>(new RestRequest());
    }

    public Author? GetAuthorById(int id)
    {
        throw new NotImplementedException();
    }

    public int InsertAuthor(Author author)
    {
        throw new NotImplementedException();
    }

    public bool UpdateAuthor(Author author)
    {
        throw new NotImplementedException();
    }
}

using RestSharp2026.ApiClient;
using System.Threading.Channels;

namespace BlogSharp2026.ApiClientConsoleTester;

internal class Program
{
    static void Main(string[] args)
    {

        //create an API client which has the URL for the API endpoint
        AuthorsApiClient _client = new AuthorsApiClient("https://localhost:7113/api/v1/authors");
        //use the api client to retrieve all authors
        var authors = _client.GetAllAuthors();
        
        //print all authors to the console
        authors.ToList().ForEach(Console.WriteLine);
        Console.ReadLine();


    }
}

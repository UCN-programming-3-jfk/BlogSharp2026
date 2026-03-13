using BlogSharp2026.DataAccess.Model;
using BlogSharp2026.DataAccess.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlogSharp2026.Test;

public class Tests
{

    const string CONNECTION_STRING = "Data Source =.;Initial Catalog=BlogSharp2026; Integrated Security = True;Trust Server Certificate = True;";

    IEnumerable<int> _cleanupIds = new List<int>();

    [TearDown]
    public void CleanUp()
    {
        //TODO: 

        //HACK: 
    }
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ShouldReturnAuthor_WhenAuthorWithThatIdExists()
    {
        //arrange

        //TODO: add code to add author
     
        AuthorDao _authorDao = new AuthorDao(CONNECTION_STRING);

        //act
        Author author = _authorDao.GetAuthorById(1);

        //assert
        Assert.That(author != null, "Author was not found");


    }
}
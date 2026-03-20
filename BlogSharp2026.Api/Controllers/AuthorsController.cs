using BlogSharp2026.DataAccess.Interfaces;
using BlogSharp2026.DataAccess.Model;
using BlogSharp2026.DataAccess.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace BlogSharp2026.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthorsController : ControllerBase
{
    IAuthorDao _authorDao;
    public AuthorsController() => _authorDao = new AuthorDao("Data Source =.;Initial Catalog=BlogSharp2026; Integrated Security = True;Trust Server Certificate = True;");

    [HttpGet]
    public ActionResult< IEnumerable<Author>> Get()
    {
        try
        {
            return Ok( _authorDao.GetAllAuthors());
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving authors");
        }
    }

    [HttpPost]
    public int InsertAuthor(Author authorToInsert)
    {
        return _authorDao.InsertAuthor(authorToInsert);
    }


}

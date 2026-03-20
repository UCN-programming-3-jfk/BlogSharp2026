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
    public ActionResult<IEnumerable<Author>> Get()
    {
        try
        {
            return Ok(_authorDao.GetAllAuthors());
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving authors", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public ActionResult<Author> Get(int id)
    {
        try
        {
            var author = _authorDao.GetAuthorById(id);
            if (author == null)
            {
                return NotFound(new { message = $"Author with id {id} not found" });
            }
            return Ok(author);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error retrieving author with id {id}", error = ex.Message });
        }
    }

    [HttpPost]
    public ActionResult<int> Post([FromBody] Author authorToInsert)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(authorToInsert.Email) || 
                string.IsNullOrWhiteSpace(authorToInsert.BlogTitle) ||
                string.IsNullOrWhiteSpace(authorToInsert.PasswordHash))
            {
                return BadRequest(new { message = "Email, BlogTitle, and Password are required" });
            }

            var newId = _authorDao.InsertAuthor(authorToInsert);
            return CreatedAtAction(nameof(Get), new { id = newId }, newId);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error inserting author", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] Author authorToUpdate)
    {
        try
        {
            if (id != authorToUpdate.Id)
            {
                return BadRequest(new { message = "Id mismatch between route and body" });
            }

            if (string.IsNullOrWhiteSpace(authorToUpdate.Email) || 
                string.IsNullOrWhiteSpace(authorToUpdate.BlogTitle))
            {
                return BadRequest(new { message = "Email and BlogTitle are required" });
            }

            var result = _authorDao.UpdateAuthor(authorToUpdate);
            if (!result)
            {
                return NotFound(new { message = $"Author with id {id} not found or not updated" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error updating author with id {id}", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            var result = _authorDao.DeleteAuthor(id);
            if (!result)
            {
                return NotFound(new { message = $"Author with id {id} not found or not deleted" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error deleting author with id {id}", error = ex.Message });
        }
    }
}

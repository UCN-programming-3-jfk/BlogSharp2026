using BlogSharp2026.DataAccess.Interfaces;
using BlogSharp2026.DataAccess.Model;
using BlogSharp2026.DataAccess.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace BlogSharp2026.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BlogPostsController : ControllerBase
{
    IBlogPostDao _blogPostDao;

    public BlogPostsController() => _blogPostDao = new BlogPostDao("Data Source =.;Initial Catalog=BlogSharp2026; Integrated Security = True;Trust Server Certificate = True;");

    [HttpGet]
    public ActionResult<IEnumerable<BlogPost>> Get(
        [FromQuery] string orderby = "", 
        [FromQuery] int amount = 10)
    {
        try
        {
            IEnumerable<BlogPost> blogPosts;

            if (orderby.Equals("latest", StringComparison.OrdinalIgnoreCase))
            {
                // Get latest posts across all authors
                blogPosts = _blogPostDao.GetLatestBlogPosts(amount);
            }
            else
            {
                // Get all posts
                blogPosts = _blogPostDao.GetAllBlogPosts();
            }

            return Ok(blogPosts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving blog posts", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public ActionResult<BlogPost> Get(int id)
    {
        try
        {
            var blogPost = _blogPostDao.GetBlogPostById(id);
            if (blogPost == null)
            {
                return NotFound(new { message = $"Blog post with id {id} not found" });
            }
            return Ok(blogPost);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error retrieving blog post with id {id}", error = ex.Message });
        }
    }


    [HttpPost]
    public ActionResult<int> Post([FromBody] BlogPost blogPostToInsert)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(blogPostToInsert.PostTitle) || 
                string.IsNullOrWhiteSpace(blogPostToInsert.PostContent))
            {
                return BadRequest(new { message = "PostTitle and PostContent are required" });
            }

            if (blogPostToInsert.FK_Author_Id <= 0)
            {
                return BadRequest(new { message = "Valid FK_Author_Id is required" });
            }

            var newId = _blogPostDao.InsertBlogPost(blogPostToInsert);
            return CreatedAtAction(nameof(Get), new { id = newId }, newId);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error inserting blog post", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] BlogPost blogPostToUpdate)
    {
        try
        {
            if (id != blogPostToUpdate.Id)
            {
                return BadRequest(new { message = "Id mismatch between route and body" });
            }

            if (string.IsNullOrWhiteSpace(blogPostToUpdate.PostTitle) || 
                string.IsNullOrWhiteSpace(blogPostToUpdate.PostContent))
            {
                return BadRequest(new { message = "PostTitle and PostContent are required" });
            }

            if (blogPostToUpdate.FK_Author_Id <= 0)
            {
                return BadRequest(new { message = "Valid FK_Author_Id is required" });
            }

            var result = _blogPostDao.UpdateBlogPost(blogPostToUpdate);
            if (!result)
            {
                return NotFound(new { message = $"Blog post with id {id} not found or not updated" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error updating blog post with id {id}", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            var result = _blogPostDao.DeleteBlogPost(id);
            if (!result)
            {
                return NotFound(new { message = $"Blog post with id {id} not found or not deleted" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error deleting blog post with id {id}", error = ex.Message });
        }
    }
}

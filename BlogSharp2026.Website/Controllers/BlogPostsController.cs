using BlogSharp2026.DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp2026.ApiClient;

namespace BlogSharp2026.Website.Controllers;

public class BlogPostsController : Controller
{
    IBlogPostDao _blogPostApiClient = new BlogPostsApiClient("https://localhost:7113/api/v1/blogposts");

    // GET: BlogPostsController
    public ActionResult Index()
    {
        return View();
    }

    // GET: BlogPostsController/Details/5
    public ActionResult Details(int id)
    {
        var blogPostToShow = _blogPostApiClient.GetBlogPostById(id);
        return View(blogPostToShow);
    }

    // GET: BlogPostsController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: BlogPostsController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: BlogPostsController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: BlogPostsController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: BlogPostsController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: BlogPostsController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}

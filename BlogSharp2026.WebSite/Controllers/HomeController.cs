using BlogSharp2026.DataAccess.Interfaces;
using BlogSharp2026.WebSite.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp2026.ApiClient;
using System.Diagnostics;

namespace BlogSharp2026.WebSite.Controllers;

public class HomeController : Controller
{
    IBlogPostDao _blogPostApiClient = new BlogPostsApiClient("https://localhost:7113/api/v1/blogposts");

    public HomeController()
    {
    
    }

    public IActionResult Index()
    {
        var tenlatestBlogPosts = _blogPostApiClient.GetLatestBlogPosts(10);
        return View(tenlatestBlogPosts);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

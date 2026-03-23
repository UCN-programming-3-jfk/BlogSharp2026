using BlogSharp2026.DataAccess.Interfaces;
using BlogSharp2026.Website.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp2026.ApiClient;
using System.Diagnostics;

namespace BlogSharp2026.Website.Controllers;

public class HomeController : Controller
{

    IBlogPostDao _blogPostApiClient = new BlogPostsApiClient("https://localhost:7113/api/v1/blogposts");

    //shows the front page with 10 latest blogposts
    public IActionResult Index()
    {
        var tenLatestBlogPosts = _blogPostApiClient.GetLatestBlogPosts(numberOfBlogPostsToRetrieve:10);
        return View(tenLatestBlogPosts);
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

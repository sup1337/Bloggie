using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bloggie.web.Models;
using Bloggie.web.Models.ViewModels;
using Bloggie.web.Repositories;

namespace Bloggie.web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBlogPostRepository blogPostRepository;
    private readonly ITagRepository tagRepository;

    public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository,
        ITagRepository tagRepository)
    {
        _logger = logger;
        this.blogPostRepository = blogPostRepository;
        this.tagRepository = tagRepository;
    }

    public async Task<IActionResult> Index()
    {
        //geting all blog posts
        var blogPosts = await blogPostRepository.GetAllAsync();

        //getting all tags
        // ReSharper disable All
        var tags = await tagRepository.GetAllAsync(null);
        // ReSharper restore All
        
        var model = new HomeViewModel
        {
            BlogPosts = blogPosts,
            Tags = tags
        };
        return View(model);
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
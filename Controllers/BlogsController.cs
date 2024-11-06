using Bloggie.web.Models.Domain;
using Bloggie.web.Models.ViewModels;
using Bloggie.web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.web.Controllers;

public class BlogsController : Controller
{
    private readonly IBlogPostRepository blogPostRepository;
    private readonly IBlogPostLikeRepository _blogPostLikeRepository;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IBlogPostCommentRepository _blogPostCommentRepository;

    public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository,
    SignInManager<IdentityUser> signInManager,
    UserManager<IdentityUser> userManager,
    IBlogPostCommentRepository blogPostCommentRepository)
    {
        this.blogPostRepository = blogPostRepository;
        _blogPostLikeRepository = blogPostLikeRepository;
        _signInManager = signInManager;
        _userManager = userManager;
        _blogPostCommentRepository = blogPostCommentRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string urlHandle)
    {
        var liked = false;
        var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);
        var blogDetailsViewModel = new BlogDetailsViewModel();
        
        if (blogPost != null)
        {
            var totalLikes = await _blogPostLikeRepository.GetTotalLikes(blogPost.Id);

            if (_signInManager.IsSignedIn(User))
            {
                //get like for this blog from this user 
               var likesForBlog = await _blogPostLikeRepository.GetLikesForBlog(blogPost.Id);
               var userId = _userManager.GetUserId(User);
               if (userId != null)
               {
                   var likeFromUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                   liked = likeFromUser != null;
               }
            }
            //get comments for this blog
            var blogCommentsDomainModel = await _blogPostCommentRepository.GetCommentsByBlogAsync(blogPost.Id);

            var blogCommentsForView = new List<BlogComment>();
            
                foreach (var blogComment in blogCommentsDomainModel)
                {
                   blogCommentsForView.Add(new BlogComment
                   {
                          Description = blogComment.Description,
                          DateAdded = blogComment.DateAdded,
                          UserName = (await _userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
                   });
                }
                
             blogDetailsViewModel = new BlogDetailsViewModel
            {
                Id = blogPost.Id,
                Heading = blogPost.Heading,
                PageTitle = blogPost.PageTitle,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                FeaturedImage = blogPost.FeaturedImage,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                Visible = blogPost.Visible,
                Tags = blogPost.Tags,
                TotalLikes = totalLikes,
                Liked = liked,
                Comments = blogCommentsForView
                
            };
        }

        return View(blogDetailsViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
    {
        if (_signInManager.IsSignedIn(User))
        {
            var domainModel = new BlogPostComment
            {
                BlogPostId = blogDetailsViewModel.Id,
                Description = blogDetailsViewModel.CommentDescription,
                UserId = Guid.Parse(_userManager.GetUserId(User)),
                DateAdded = DateTime.Now
            };
            await _blogPostCommentRepository.AddAsync(domainModel);
            return RedirectToAction("Index", "Blogs",
                new {urlHandle = blogDetailsViewModel.UrlHandle});
        }

        return View();
    }
    
    
    
}
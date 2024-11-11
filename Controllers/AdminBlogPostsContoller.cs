using Bloggie.web.Models.Domain;
using Bloggie.web.Models.ViewModels;
using Bloggie.web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.web.Controllers;

[Authorize (Roles = "Admin")]

public class AdminBlogPostsController : Controller
{
    private readonly ITagRepository tagRepository;
    private readonly IBlogPostRepository blogPostRepository;

    public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
    {
        this.tagRepository = tagRepository;
        this.blogPostRepository = blogPostRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        //get tags from repo 
        var tags = await tagRepository.GetAllAsync();

        var model = new AddBlogPostRequest
        {
            Tags = tags.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
    {
        var blogPost = new BlogPost
        {
            Heading = addBlogPostRequest.Heading,
            PageTitle = addBlogPostRequest.PageTitle,
            Content = addBlogPostRequest.Content,
            ShortDescription = addBlogPostRequest.ShortDescription,
            FeaturedImage = addBlogPostRequest.FeaturedImage,
            UrlHandle = addBlogPostRequest.UrlHandle,
            PublishedDate = addBlogPostRequest.PublishedDate,
            Author = addBlogPostRequest.Author,
            Visible = addBlogPostRequest.Visible,
        };
        var selectedTags = new List<Tag>();
        foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
        {
            var selectedTagIdGuid = Guid.Parse(selectedTagId);
            var existingTag = await tagRepository.GetAsync(selectedTagIdGuid);

            if (existingTag != null)
            {
                selectedTags.Add(existingTag);
            }
        }

        //mapping tags back to domain model
        blogPost.Tags = selectedTags;

        await blogPostRepository.AddAsync(blogPost);
        return RedirectToAction("Add");
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        //call the repo to get all blog posts
        var blogPosts = await blogPostRepository.GetAllAsync();
        return View(blogPosts.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        //retrive reesult from repo
        var blogPost = await blogPostRepository.GetAsync(id);
        var tagsDomainModel = await tagRepository.GetAllAsync();

        if (blogPost != null)
        {
            //map domain model to view model
            var model = new EditBlogPostRequest
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
                Tags = tagsDomainModel.Select(x => new SelectListItem
                {
                    Text = x.Name, Value = x.Id.ToString()
                }),
                SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
            };
            return View(model);
        }

        //pass data to view
        return View(null);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
    {
        //map view model back to domain 
        var blogPostDomainModel = new BlogPost
        {
            Id = editBlogPostRequest.Id,
            Heading = editBlogPostRequest.Heading,
            PageTitle = editBlogPostRequest.PageTitle,
            Content = editBlogPostRequest.Content,
            ShortDescription = editBlogPostRequest.ShortDescription,
            FeaturedImage = editBlogPostRequest.FeaturedImage,
            UrlHandle = editBlogPostRequest.UrlHandle,
            PublishedDate = editBlogPostRequest.PublishedDate,
            Author = editBlogPostRequest.Author,
            Visible = editBlogPostRequest.Visible,
        };
        //mao tags into domain model
        var selectedTags = new List<Tag>();
        foreach (var selectedTag in editBlogPostRequest.SelectedTags)
        {
            if (Guid.TryParse(selectedTag, out var tag))
            {
                var foundTag = await tagRepository.GetAsync(tag);
                if (foundTag != null)
                {
                    selectedTags.Add(foundTag);
                }
            }
        }

        blogPostDomainModel.Tags = selectedTags;


        //submit to repo
        var updatedBlog = await blogPostRepository.UpdateAsync(blogPostDomainModel);
        if (updatedBlog != null)
        {
            //show success message
            return RedirectToAction("Edit");
        }

        //redirect to get
        return RedirectToAction("Edit");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
    {
     //talk to repo to delete bolgpost and related tags 
    var deletedBlogPost =  await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);
    if (deletedBlogPost != null)
    {
        //show success message
        return RedirectToAction("List");
    }
    //show error message
    return RedirectToAction("Edit",new {id = editBlogPostRequest.Id});
     
     //display response
     
     
    }
}
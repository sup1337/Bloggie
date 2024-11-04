using Bloggie.web.Data;
using Bloggie.web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.web.Repositories;

public class BlogPostLikeRepository : IBlogPostLikeRepository
{
    private readonly BloggieDbContext _bloggieDbContext;

    public BlogPostLikeRepository(BloggieDbContext bloggieDbContext)
    {
        _bloggieDbContext = bloggieDbContext;
    }
    
    public async Task<int> GetTotalLikes(Guid blogPostId)
    {
       return await _bloggieDbContext.BlogPostLike.CountAsync(x => x.BlogPostId == blogPostId);
    }

    public async Task<BlogPostLike> AddLikeForBlogPost(BlogPostLike blogPostLike)
    {
        await _bloggieDbContext.BlogPostLike.AddAsync(blogPostLike);
        await _bloggieDbContext.SaveChangesAsync();
        return blogPostLike;
    }
}
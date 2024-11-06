using Bloggie.web.Models.Domain;

namespace Bloggie.web.Repositories;

public interface IBlogPostCommentRepository
{
    Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);
    
    Task<IEnumerable<BlogPostComment>> GetCommentsByBlogAsync(Guid blogPostId);
}
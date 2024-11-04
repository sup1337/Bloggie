using Bloggie.web.Models.Domain;
using Bloggie.web.Models.ViewModels;

namespace Bloggie.web.Repositories;

public interface IBlogPostLikeRepository
{
    Task<int> GetTotalLikes(Guid blogPostId);
    
    Task<BlogPostLike>AddLikeForBlogPost(BlogPostLike blogPostLike);
}
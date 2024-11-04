using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.web.Models.ViewModels;

public class EditBlogPostRequest
{
   
    public Guid Id { get; set; }
    
    public string Heading { get; set; }

    public string PageTitle { get; set; }

    public string Content { get; set; }  
    
    public string ShortDescription { get; set; }
    
    public string FeaturedImage { get; set; }

    public string UrlHandle { get; set; }

    public DateTime PublishedDate { get; set; }
    
    public string Author { get; set; }
    
    public bool Visible { get; set; }

    
    //display tags
    public IEnumerable<SelectListItem> Tags { get; set; }
    //collect tags
    public string[] SelectedTags { get; set; } = Array.Empty<string>();
}
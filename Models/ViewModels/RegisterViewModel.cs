using System.ComponentModel.DataAnnotations;

namespace Bloggie.web.Models.ViewModels;

public class RegisterViewModel
{
    [Microsoft.Build.Framework.Required]
    public string Username { get; set; }
    
    [Microsoft.Build.Framework.Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Microsoft.Build.Framework.Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; }
    
}
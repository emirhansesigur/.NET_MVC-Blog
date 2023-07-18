using Microsoft.AspNetCore.Identity;

namespace AdminBlog.Controllers
{
    internal class ApplicationUser : IdentityUser
    {
        public string UserName { get; set; }
    }

}
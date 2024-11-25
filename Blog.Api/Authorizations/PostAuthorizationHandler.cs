using Blog.Api.Authorizations;
using Blog.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Api.Authorizations;

public class PostAuthorizationHandler : AuthorizationHandler<PostAuthorizationRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly PostService _postService;

    public PostAuthorizationHandler(IHttpContextAccessor httpContextAccessor, PostService postService)
    {
        _httpContextAccessor = httpContextAccessor;
        _postService = postService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PostAuthorizationRequirement requirement)
    {
        var user = context.User;
        if (user.IsInRole("Admin"))
        {
            context.Succeed(requirement);
            return;
        }

        var routeData = _httpContextAccessor.HttpContext.GetRouteData();

        var isCreateAction = routeData.Values["Action"].ToString().Contains("Create");
        var idValue = routeData.Values["id"];

        if (isCreateAction && user.Identity.IsAuthenticated)
        {
            context.Succeed(requirement);
            return;
        }

        if  (routeData.Values["id"] is string idString && int.TryParse(idString, out int postId)) 
        {
            var postAuthor = await _postService.GetPostAuthorByIdAsync(postId);
            var post = await _postService.GetPostByIdAsync(postId);
            if (post != null && postAuthor.AuthorEmail == user.FindFirstValue(ClaimTypes.Email))
            {
                context.Succeed(requirement);
                return;
            }
        }

        context.Fail();
    }
}

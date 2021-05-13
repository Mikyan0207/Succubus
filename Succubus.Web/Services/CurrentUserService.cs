using Microsoft.AspNetCore.Http;
using Succubus.Application.Interfaces;
using System.Security.Claims;

namespace Succubus.Web.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private IHttpContextAccessor HttpContextAccessor { get; }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public string UserId => HttpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
}
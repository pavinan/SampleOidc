using Microsoft.AspNetCore.Http;

namespace SampleOidc.Contacts.API.Services
{
    public class IdentityService
    {
        private readonly IHttpContextAccessor _context;

        public IdentityService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string GetUserIdentity()
        {
            return _context.HttpContext.User.FindFirst("sub").Value;
        }
    }
}
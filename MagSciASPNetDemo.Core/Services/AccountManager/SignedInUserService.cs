using ContactsManagement.Core.ServiceContracts.AccountManager;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.AccountManager
{
    /// <summary>
    /// Note that in order to use the HttpContext class in a service, you need to register the IHttpContextAccessor interface with the dependency injection system.
    /// Overall, the performance difference between the UserManager and SignedInUserService approach is likely to be negligible, however, it's recommended to use the former for consistency and best practice reasons.
    /// </summary>
    public class SignedInUserService : ISignedInUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid? UserId { get; set; }
        public SignedInUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor; 
        }
        public Guid? GetSignedInUserId()
        {
            if (this.UserId != null)
                return this.UserId;

            var claimsPrincipal = _httpContextAccessor.HttpContext.User;
            var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

            // Extract the user's ID value
            if (userIdClaim == null)
                return null;
            var userId = userIdClaim?.Value;
            this.UserId = Guid.Parse(userId);
            return UserId;
        }
    }
}

using ContactsManagement.Core.Domain.IdentityEntities;

namespace ContactsManagement.Core.ServiceContracts.AccountManager
{
    /// <summary>
    /// Actually.. possible? Through Claims Principal or HttpContext pero meh..
    /// </summary>
    public interface ILoggedInUserService
    {
        Task<Guid> GetUserId();
        Task<ApplicationUser> GetUser();
    }
}

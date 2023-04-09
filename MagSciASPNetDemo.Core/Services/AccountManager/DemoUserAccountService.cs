using ContactsManagement.Core.ServiceContracts.AccountManager;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.AccountManager
{
    public class DemoUserAccountService : IDemoUserService
    {
        private readonly Guid _guestUserId;
        public DemoUserAccountService(IConfiguration config)
        {
            string? userId = config["DemoUserId"].ToString();
            _guestUserId= Guid.Parse(userId);
        }
        public Guid GetDemoUserId()
        {
            return _guestUserId;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Exceptions
{
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException() : base("Access denied for non-registered user.")
        {
        }
        public AccessDeniedException(string message) : base(message)
        {
        }
    }
}

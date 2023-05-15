using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Exceptions
{
    public class NullUserIDException : ArgumentException
    {
        public NullUserIDException() : base("UserID is null")
        {
        }
        public NullUserIDException(string message) : base(message)
        {

        }
    }
}

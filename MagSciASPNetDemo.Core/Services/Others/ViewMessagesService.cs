using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.ServiceContracts.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.Others
{
    public class ViewMessagesService : IViewMessagesService
    {
        List<Tuple<RequestStatusType, string>> StatusMessages;
        public ViewMessagesService() 
        {
            StatusMessages = new List<Tuple<RequestStatusType, string>>();
        }
        public void AddMessage(string message, RequestStatusType statusType)
        {
            StatusMessages.Add(new Tuple<RequestStatusType, string>(statusType, message));
        }

        public bool ContainsMessage(RequestStatusType statusType)
        {
            return StatusMessages.Any(temp => temp.Item1 == statusType) ? true : false;
        }

        public List<string>? GetMessages(RequestStatusType statusType)
        {
            return StatusMessages.Where(temp => temp.Item1 == statusType).Select(temp => temp.Item2).ToList();
        }
    }
}

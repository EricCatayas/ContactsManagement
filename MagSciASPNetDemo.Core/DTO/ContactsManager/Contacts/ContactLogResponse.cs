using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.DTO.ContactsManager.Contacts
{
    public class ContactLogResponse
    {
        public int LogId { get; set; }
        public string? Type { get; set; }
        [DisplayName("Title")]
        public string? LogTitle { get; set; }
        public string? Note { get; set; }
        public Guid? PersonId { get; set; }
        public string? PersonLog { get; set; }
        public DateTime? DateCreated { get; set; }
    }
    public static class ContactLogResponseExtentions
    {
        public static ContactLogResponse ToContactLogResponse(this ContactLog contactLog)
        {
            return new ContactLogResponse()
            {
                LogId = contactLog.LogId,
                Type = contactLog.Type,
                LogTitle = contactLog.LogTitle,
                Note = contactLog.Note,
                PersonId = contactLog.PersonId,
                PersonLog = contactLog.PersonLog?.Name,
                DateCreated = contactLog.DateCreated,
            };
        }
        public static ContactLogUpdateRequest ToContactLogUpdateRequest(this ContactLogResponse contactLog)
        {
            return new ContactLogUpdateRequest()
            {
                LogId = contactLog.LogId,
                Type = contactLog.Type,
                LogTitle = contactLog.LogTitle,
                Note = contactLog.Note,
                PersonId = contactLog.PersonId,
                DateCreated= contactLog.DateCreated,
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.Entities.ContactsManager
{
    public class FileAttachment
    {
        [Key]
        public int FileId { get; set; }
        [MaxLength(255)]
        public string FileName { get; set; }
        [MaxLength(2048)]
        public string BlobUrl { get; set; }
        public int ContactLogId { get; set; } //Just in case
    }
}

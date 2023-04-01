using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.DTO.ContactsManager.Contacts
{
    public class FileAttachmentDTO
    {
        [Key]
        [Required]
        public int FileId { get; set; }
        [MaxLength(255)]
        [Required]
        public string FileName { get; set; }
        [MaxLength(2048)]
        [Required]
        public string BlobUrl { get; set; }
        public int ContactLogId { get; set; }
    }
    public static class FileAttachmentDTOExtentions
    {
        public static FileAttachmentDTO ToFileAttachmentDTO(this FileAttachment fileAttachment)
        {
            return new FileAttachmentDTO()
            {
                FileId = fileAttachment.FileId,
                FileName = fileAttachment.FileName,
                BlobUrl = fileAttachment.BlobUrl,
                ContactLogId = fileAttachment.ContactLogId,
            };
        }
    }
}

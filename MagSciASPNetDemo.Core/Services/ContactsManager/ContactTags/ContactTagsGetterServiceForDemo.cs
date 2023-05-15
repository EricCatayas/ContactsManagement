using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactTagsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.ContactsManager.ContactTags
{
    public class ContactTagsGetterServiceForDemo : IContactTagsGetterService
    {
        private readonly IContactTagsGetterRepository _contactTagsGetterRepository;
        private readonly ISignedInUserService _signedInUserService;
        private readonly IDemoUserService _demoUserService;

        public ContactTagsGetterServiceForDemo(IContactTagsGetterRepository contactTagsGetterRepository, ISignedInUserService signedInUserService, IDemoUserService demoUserService)
        {
            _contactTagsGetterRepository = contactTagsGetterRepository;
            _signedInUserService = signedInUserService;
            _demoUserService = demoUserService;
        }
        public async Task<List<ContactTagDTO>?> GetAllContactTags()
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                userId = _demoUserService.GetDemoUserId();

            List<ContactTag>? contactTags = await _contactTagsGetterRepository.GetAllContactTags((Guid)userId);
            return contactTags?.Select(tag => tag.ToContactTagResponse()).ToList();
        }
    }
}

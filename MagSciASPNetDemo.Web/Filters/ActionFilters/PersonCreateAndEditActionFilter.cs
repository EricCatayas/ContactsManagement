using ContactsManagement.Core.Domain.IdentityEntities;
using ContactsManagement.Core.DTO.CompaniesManagement;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.ServiceContracts.CompaniesManagement;
using ContactsManagement.Core.ServiceContracts.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactTagsServices;
using ContactsManagement.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ContactsManagement.Web.Filters.ActionFilters
{
    /// <summary>
    /// If there are validation errors, do not invoke ActionMethod
    /// </summary>
    public class PersonCreateAndEditActionFilter : IAsyncActionFilter
    {
        private readonly ICountriesService _countriesService;
        private readonly ICompanyGetterService _companiesService;
        private readonly IContactTagsGetterService _contactTagsGetterService;
        private readonly IContactGroupsGetterService _contactGroupsGetterService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PersonCreateAndEditActionFilter(ICountriesService countriesService, ICompanyGetterService companiesService, IContactTagsGetterService contactTagsService, IContactGroupsGetterService contactGroupsGetterService, UserManager<ApplicationUser> userManager)
        {
            _countriesService = countriesService;
            _companiesService = companiesService;
            _contactTagsGetterService = contactTagsService;
            _contactGroupsGetterService = contactGroupsGetterService;
            _userManager = userManager;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           if (context.Controller is PersonsController personController)
            {
                ClaimsPrincipal claimsPrincipal = context.HttpContext.User;
                // Get the user's ID claim
                var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

                // Extract the user's ID value
                var userId = userIdClaim?.Value;
                Guid UserId = Guid.Parse(userId);

                //ViewBag.Countries
                List<CountryResponse> countries = await _countriesService.GetAllCountries();
                personController.ViewBag.Countries = countries.Select(country => new SelectListItem()
                {
                    Text = country.CountryName,
                    Value = country.CountryId.ToString()
                }).ToList();
                //ViewBag.Companies
                List<CompanyResponse>? companies = await _companiesService.GetAllCompanies();
                if (companies != null)
                    personController.ViewBag.Companies = companies.Select(company => new SelectListItem()
                    {
                        Text = company.CompanyName,
                        Value = company.CompanyId.ToString()
                    });
                //ViewBag.ContactTags
                List<ContactTagDTO>? contactTags = await _contactTagsGetterService.GetAllContactTags();
                if (contactTags != null)
                    personController.ViewBag.ContactTags = contactTags;

                //ViewBag.ContactGroups
                List<ContactGroupResponse>? contactGroups = await _contactGroupsGetterService.GetAllContactGroups();
                personController.ViewBag.ContactGroups = contactGroups;

                if (!personController.ModelState.IsValid)
                {
                    personController.ViewBag.Errors = personController.ModelState.Values.SelectMany(V => V.Errors).Select(err => err.ErrorMessage).ToList();

                    if (context.ActionArguments.ContainsKey("personAddRequest"))
                    {
                        PersonAddRequest? personAddRequest = context.ActionArguments["personAddRequest"] as PersonAddRequest;
                        context.Result = personController.View(personAddRequest);
                    }
                    else if (context.ActionArguments.ContainsKey("personUpdateRequest"))
                    {
                        PersonUpdateRequest? personUpdateRequest = context.ActionArguments["personUpdateRequest"] as PersonUpdateRequest;
                        context.Result = personController.View(personUpdateRequest);
                    }
                    return;
                }
                else
                {
                    await next();
                }
            }

        }
    }
}

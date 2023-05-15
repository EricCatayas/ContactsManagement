using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using OfficeOpenXml;
using Serilog;
using SerilogTimings;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using Microsoft.AspNetCore.Identity;
using ContactsManagement.Core.Domain.IdentityEntities;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.Exceptions;

namespace ContactsManagement.Core.Services.ContactsManager.Persons
{
    public class PersonsGetterService : IPersonsGetterService
    {
        private readonly IPersonsGetterRepository _personsGetterRepository;
        private readonly IDiagnosticContext _diagnosticContext;
        private readonly ISignedInUserService _signedInUserService;

        public PersonsGetterService(IPersonsGetterRepository personsRepository, IDiagnosticContext diagnostics, ISignedInUserService signedInUserService)
        {
            _personsGetterRepository = personsRepository;
            _diagnosticContext = diagnostics;
            _signedInUserService = signedInUserService;

        }
        public async Task<List<PersonResponse>> GetAllPersons()
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            List<Person> persons = await _personsGetterRepository.GetAllPersons((Guid)userId);
            return persons.Select(p => p.ToPersonResponse()).ToList();
        }
        public async Task<PersonResponse?> GetPersonById(Guid? personId)
        {
            if (personId == null || personId == Guid.Empty) return null;
            Person? matchingPerson = await _personsGetterRepository.GetPersonById((Guid)personId); ;

            if (matchingPerson == null)
                throw new InvalidPersonIDException("Person with matching ID is not found");
            return matchingPerson.ToPersonResponse();
        }
        public async Task<List<PersonResponse>> GetFilteredPersons(string? searchProperty, string? searchString = "")
        {
            Guid userId = _signedInUserService.GetSignedInUserId() ?? Guid.Empty;
            if (userId == Guid.Empty)
            {
                throw new AccessDeniedException();
            }
            List<Person> persons = new List<Person>() { };
            using (Operation.Time("Time for GetFilteredPersons in PersonService")) // <-- SerilogTimings
            {
                try
                {
                    if (searchString != null)
                    {
                        persons = searchProperty switch
                        {
                            nameof(PersonResponse.PersonName) =>
                             await _personsGetterRepository.GetFilteredPersons(temp =>
                             temp.Name.Contains(searchString), userId),

                            nameof(PersonResponse.Email) =>
                             await _personsGetterRepository.GetFilteredPersons(temp =>
                             temp.Email.Contains(searchString), userId),

                            nameof(PersonResponse.JobTitle) =>
                             await _personsGetterRepository.GetFilteredPersons(temp =>
                             temp.JobTitle.Contains(searchString), userId),

                            nameof(PersonResponse.CompanyName) =>
                             await _personsGetterRepository.GetFilteredPersons(temp =>
                             temp.Company.CompanyName.Contains(searchString), userId),

                            nameof(PersonResponse.Address) =>
                            await _personsGetterRepository.GetFilteredPersons(temp =>
                            temp.Address.Contains(searchString), userId),

                            nameof(PersonResponse.CountryName) =>
                             await _personsGetterRepository.GetFilteredPersons(temp =>
                             temp.Country.CountryName.Contains(searchString), userId),

                            nameof(ContactTagDTO.TagName) =>
                           await _personsGetterRepository.GetFilteredPersons(temp =>
                           temp.Tag.TagName.Contains(searchString), userId),

                            _ => await _personsGetterRepository.GetAllPersons(userId)
                        };
                    }
                    else
                    {
                        persons = await _personsGetterRepository.GetAllPersons(userId);
                    }
                }
                catch (Exception ex)
                {
                    return new List<PersonResponse>() { };
                }
            }
            _diagnosticContext.Set("Filtered Persons: ", persons);
            return persons.Select(person => person.ToPersonResponse()).ToList();
        }
        public async Task<List<PersonResponse>?> GetPersonsById(List<Guid>? personIDs)
        {
            List<Person>? matchingPersons = await _personsGetterRepository.GetPersonsById(personIDs);
            _diagnosticContext.Set("Matching Persons", matchingPersons);
            return matchingPersons?.Select(person => person.ToPersonResponse()).ToList();
        }

        public async Task<MemoryStream> GetPersonsCSV()
        {
            MemoryStream memoryStream = new();
            StreamWriter streamWriter = new(memoryStream); // The StreamWriter writes the stream to the memoryStream

            CsvConfiguration csvConfiguration = new(CultureInfo.InvariantCulture);
            // It needs to write the content into the streamWriter, and Current Working Culture .InvariantCulture, after writign a 100 bytes, you have to start at the beginning of the stream - so keep stream open
            CsvWriter csvWriter = new(streamWriter, csvConfiguration, leaveOpen: true);
            csvWriter.WriteField(nameof(PersonResponse.PersonName));
            csvWriter.WriteField(nameof(PersonResponse.Email));
            csvWriter.WriteField(nameof(PersonResponse.JobTitle));
            csvWriter.WriteField(nameof(PersonResponse.Address));
            csvWriter.WriteField(nameof(PersonResponse.CountryName));
            csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
            csvWriter.WriteField(nameof(PersonResponse.Gender));
            csvWriter.WriteField(nameof(PersonResponse.ContactNumber1));
            csvWriter.WriteField(nameof(PersonResponse.ContactNumber2));            
            csvWriter.WriteField(nameof(PersonResponse.ProfileBlobUrl));            
            csvWriter.NextRecord();
            List<PersonResponse> allPersons = await GetAllPersons();
            allPersons.ForEach(person =>
            {
                csvWriter.WriteField(person.PersonName);
                csvWriter.WriteField(person.Email);
                csvWriter.WriteField(person.JobTitle);
                csvWriter.WriteField(person.Address);
                csvWriter.WriteField(person.CountryName);
                csvWriter.WriteField(person.DateOfBirth?.ToString("MM dd yyyy"));
                csvWriter.WriteField(person.Gender);
                csvWriter.WriteField(person.ContactNumber1);
                csvWriter.WriteField(person.ContactNumber2);
                csvWriter.WriteField(person.ProfileBlobUrl);
                csvWriter.NextRecord();
                csvWriter.Flush(); // Flush so that it gets added to the memoryStream
            });
            // await csvWriter.WriteRecordsAsync(await this.GetAllPersons()); We don't want to display all properties
            memoryStream.Position = 0;

            return memoryStream;
        }
        // Nuget: EPPlus
        public async Task<MemoryStream> GetPersonsEXCEL()
        {
            MemoryStream memoryStream = new();

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Persons");
                worksheet.Cells[1, 1].Value = "Person Name";
                worksheet.Cells[1, 2].Value = "Email";
                worksheet.Cells[1, 3].Value = "Title";
                worksheet.Cells[1, 4].Value = "BirthDate";
                worksheet.Cells[1, 5].Value = "Age";
                worksheet.Cells[1, 6].Value = "Gender";
                worksheet.Cells[1, 7].Value = "Address";
                worksheet.Cells[1, 8].Value = "Country";
                worksheet.Cells[1, 9].Value = "Company";
                worksheet.Cells[1, 10].Value = "Contact Number 1";
                worksheet.Cells[1, 11].Value = "Contact Number 2";
                worksheet.Cells[1, 12].Value = "Profile Url";

                using (ExcelRange headerCells = worksheet.Cells["A1:L1"])
                {
                    headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    headerCells.Style.Font.Bold = true;
                }
                int row = 2;
                List<PersonResponse> persons = await GetAllPersons();
                foreach (PersonResponse person in persons)
                {
                    worksheet.Cells[row, 1].Value = person.PersonName;
                    worksheet.Cells[row, 2].Value = person.Email;
                    worksheet.Cells[row, 3].Value = person.JobTitle;
                    worksheet.Cells[row, 4].Value = person.DateOfBirth?.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 5].Value = person.Age;
                    worksheet.Cells[row, 6].Value = person.Gender;
                    worksheet.Cells[row, 7].Value = person.Address;
                    worksheet.Cells[row, 8].Value = person.CountryName;
                    worksheet.Cells[row, 9].Value = person.CompanyName;
                    worksheet.Cells[row, 10].Value = person.ContactNumber1;
                    worksheet.Cells[row, 11].Value = person.ContactNumber2;
                    worksheet.Cells[row, 12].Value = person.ProfileBlobUrl;

                    row++;
                }
                worksheet.Cells[$"A1:L{row}"].AutoFitColumns();
                await excelPackage.SaveAsync();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}

using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.DTO.ContactsManager;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.ContactsManager
{
    /// <summary>
    /// Demo for Open-Close Principle using Interface
    /// Another approach is through inheritance
    /// </summary>
    public class PersonsGetterServiceModifiedExcel : IPersonsGetterService
    {
        private readonly IPersonsGetterService _personsGetterService;
        public PersonsGetterServiceModifiedExcel(IPersonsGetterService personsGetterService)
        {
            _personsGetterService = personsGetterService;
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            return await _personsGetterService.GetAllPersons();
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string? searchProperty, string? searchString)
        {
            return await _personsGetterService.GetFilteredPersons(searchProperty, searchString);
        }

        public async Task<PersonResponse?> GetPersonById(Guid? personId)
        {
            return await GetPersonById(personId);
        }

        public Task<List<PersonResponse>?> GetPersonsById(List<Guid>? personId)
        {
            throw new NotImplementedException();
        }

        public async Task<MemoryStream> GetPersonsCSV()
        {
            return await _personsGetterService.GetPersonsCSV();
        }

        public async Task<MemoryStream> GetPersonsEXCEL()
        {
            MemoryStream memoryStream = new();

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Persons");
                worksheet.Cells[1, 1].Value = "Person Name";
                worksheet.Cells[1, 2].Value = "Email";
                worksheet.Cells[1, 3].Value = "Address";

                using (ExcelRange headerCells = worksheet.Cells["A1:C1"])
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
                    worksheet.Cells[row, 3].Value = person.Address;

                    row++;
                }
                worksheet.Cells[$"A1:C{row}"].AutoFitColumns();
                await excelPackage.SaveAsync();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}

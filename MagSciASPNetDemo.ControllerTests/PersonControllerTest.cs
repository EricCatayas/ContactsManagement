﻿using AutoFixture;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using ContactsManagement.Core.ServiceContracts.ContactsManager;
using ContactsManagement.Web.Controllers;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;

namespace xUnitTesting
{
    public class PersonControllerTest
    {
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly ICountriesService _countriesService;

        private readonly Mock<ICountriesService> _countriesServiceMock;
        private readonly Mock<IPersonsGetterService> _personsGetterServiceMock;
        private readonly Mock<IPersonsAdderService> _personsAdderServiceMock;
        private readonly Mock<IPersonsDeleterService> _personsDeleterServiceMock;
        private readonly Mock<IPersonsSorterService> _personsSorterServiceMock;
        private readonly Mock<IPersonsUpdaterService> _personsUpdaterServiceMock;

        private readonly Fixture _fixture;
        private readonly ILogger<PersonController> _personsControllerLogger;

        public PersonControllerTest()
        {
            _countriesServiceMock = new Mock<ICountriesService>();
            _personsGetterServiceMock = new Mock<IPersonsGetterService>();
            _personsAdderServiceMock = new Mock<IPersonsAdderService>();
            _personsDeleterServiceMock = new Mock<IPersonsDeleterService>();
            _personsSorterServiceMock = new Mock<IPersonsSorterService>();
            _personsUpdaterServiceMock = new Mock<IPersonsUpdaterService>();

            _countriesService = _countriesServiceMock.Object;
            _personsGetterService = _personsGetterServiceMock.Object;
            _personsAdderService = _personsAdderServiceMock.Object;
            _personsDeleterService = _personsDeleterServiceMock.Object;
            _personsSorterService = _personsSorterServiceMock.Object;
            _personsUpdaterService = _personsUpdaterServiceMock.Object;

            _fixture = new Fixture();
            Mock<ILogger<PersonController>> _personControllerLoggerMock = new Mock<ILogger<PersonController>>();
            _personsControllerLogger = _personControllerLoggerMock.Object;
        }

        #region Index

        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPersonsList()
        {
            //Arrange
            List<PersonResponse> persons_response_list = _fixture.Create<List<PersonResponse>>();

            PersonController PersonController = new PersonController(_personsGetterService, _personsAdderService, _personsDeleterService, _personsSorterService, _personsUpdaterService, _countriesService, _personsControllerLogger);

            _personsGetterServiceMock
             .Setup(temp => temp.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>()))
             .ReturnsAsync(persons_response_list);

            _personsSorterServiceMock
             .Setup(temp => temp.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>()))
             .ReturnsAsync(persons_response_list);

            //Act
            IActionResult result = await PersonController.Index(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<SortOrderOptions>().ToString());

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
            viewResult.ViewData.Model.Should().Be(persons_response_list);
        }
        #endregion


        #region Create

        [Fact]
        public async void Create_IfModelErrors_ToReturnCreateView()
        {
            //Arrange
            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
                .With(person => person.Email, "some@gmail.com")
                .With(person => person.PersonName, "")
                .With(person => person.Gender, Gender.Get("Male"))
                .With(person => person.DateOfBirth, DateTime.Parse("2020-12-01"))
                .With(person => person.CountryId, Guid.NewGuid()).Create();

            PersonResponse person_response = person_add_request.ToPerson().ToPersonResponse();

            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

            _countriesServiceMock
             .Setup(temp => temp.GetAllCountries())
             .ReturnsAsync(countries);

            _personsAdderServiceMock
             .Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
             .ReturnsAsync(person_response);

            PersonController personController = new PersonController(_personsGetterService, _personsAdderService, _personsDeleterService, _personsSorterService, _personsUpdaterService, _countriesService, _personsControllerLogger);


            //Act
            personController.ViewBag.Countries = countries;
            personController.ModelState.AddModelError("PersonName", "Person Name can't be blank"); //Custom model error

            IActionResult result = await personController.Create(person_add_request);

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();

            viewResult.ViewData.Model.Should().Be(person_add_request);
        }


        [Fact]
        public async void Create_IfNoModelErrors_ToReturnCreateView()
        {
            //Arrange
            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
                .With(person => person.Email, "some@gmail.com")
                .With(person => person.PersonName, "jojobart")
                .With(person => person.Gender, Gender.Get("Male"))
                .With(person => person.DateOfBirth, DateTime.Parse("2000-12-01"))
                .With(person => person.CountryId, Guid.NewGuid()).Create();

            PersonResponse person_response = person_add_request.ToPerson().ToPersonResponse();

            _personsAdderServiceMock
             .Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
             .ReturnsAsync(person_response);

            PersonController PersonController = new PersonController(_personsGetterService, _personsAdderService, _personsDeleterService, _personsSorterService, _personsUpdaterService, _countriesService, _personsControllerLogger);

            //Act
            IActionResult result = await PersonController.Create(person_add_request);

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
        }

        #endregion
    }
}

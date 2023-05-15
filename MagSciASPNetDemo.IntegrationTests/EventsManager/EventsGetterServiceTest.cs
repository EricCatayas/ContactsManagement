using ContactsManagement.Core.Domain.RepositoryContracts.EventsManager;
using ContactsManagement.Core.DTO.EventsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.EventsManager;
using ContactsManagement.Core.Services.EventsManager;
using ContactsManagement.Infrastructure.DbContexts;
using ContactsManagement.Infrastructure.Repositories.EventsManager;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.IntegrationTests.EventsManager
{
    public class EventsGetterServiceTest
    {
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;
        public EventsGetterServiceTest()
        {
            _mockSignedInUserService = new Mock<ISignedInUserService>();
        }
        [Fact]
        public async void GetEventById_ToReturnObject()
        {
            //Arrange
            Guid UserID = Guid.NewGuid();
            DateTime dateNow = DateTime.Now;
            EventAddRequest eventAddRequest = new EventAddRequest()
            {
                Title = "SampleTest",
                Description = "SampleTest",
                Type = "SampleTest",
                ThemeColor = EventColorOptions.orange.ToString(),
                isActive = true,
                StartDate = dateNow,
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                bool isCreated = context.Database.EnsureCreated();

                IEventsAdderRepository eventsAdderRepository = new EventsAdderRepository(context);
                IEventsGetterRepository eventsGetterRepository = new EventsGetterRepository(context);
                IEventsStatusUpdaterRepository eventsStatusUpdaterRepository = new EventsStatusUpdaterRepository(context);
                IEventsAdderService eventsAdderService = new EventsAdderService(eventsAdderRepository, eventsStatusUpdaterRepository, _mockSignedInUserService.Object);
                IEventsGetterService _eventsGetterService = new EventsGetterService(eventsGetterRepository, eventsStatusUpdaterRepository, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserID);

                //Act
                EventResponse event_FromAddEvent = await eventsAdderService.AddEvent(eventAddRequest);
                EventResponse? events_FromGetEventById = await _eventsGetterService.GetEventById(event_FromAddEvent.EventId);

                //Assert
                Assert.NotNull(events_FromGetEventById.Status);
                Assert.True(events_FromGetEventById.Title == eventAddRequest.Title &&
                            events_FromGetEventById.Type == eventAddRequest.Type &&
                            events_FromGetEventById.ThemeColor == eventAddRequest.ThemeColor &&
                            events_FromGetEventById.StartDate == eventAddRequest.StartDate);

            }
        }
        [Fact]
        public async void GetEvents_ToReturnAllEvents()
        {
            //Arrange
            Guid UserID = Guid.NewGuid();
            DateTime dateNow = DateTime.Now;
            EventAddRequest event1 = new EventAddRequest()
            {
                Title = "SampleTest",
                Description = "SampleTest",
                Type = "SampleTest",
                ThemeColor = EventColorOptions.orange.ToString(),
                isActive = true,
                StartDate = dateNow,
            };
            EventAddRequest event2 = new EventAddRequest()
            {
                Title = "SampleTest",
                Description = "SampleTest",
                Type = "SampleTest",
                ThemeColor = EventColorOptions.orange.ToString(),
                isActive = true,
                StartDate = dateNow,
            };
            EventAddRequest event3 = new EventAddRequest()
            {
                Title = "SampleTest",
                Description = "SampleTest",
                Type = "SampleTest",
                ThemeColor = EventColorOptions.orange.ToString(),
                isActive = true,
                StartDate = dateNow,
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                bool isCreated = context.Database.EnsureCreated();

                IEventsAdderRepository eventsAdderRepository = new EventsAdderRepository(context);
                IEventsGetterRepository eventsGetterRepository = new EventsGetterRepository(context);
                IEventsStatusUpdaterRepository eventsStatusUpdaterRepository = new EventsStatusUpdaterRepository(context);
                IEventsAdderService eventsAdderService = new EventsAdderService(eventsAdderRepository, eventsStatusUpdaterRepository, _mockSignedInUserService.Object);
                IEventsGetterService _eventsGetterService = new EventsGetterService(eventsGetterRepository, eventsStatusUpdaterRepository, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserID);

                //Act
                _ = await eventsAdderService.AddEvent(event1);
                _ = await eventsAdderService.AddEvent(event2);
                _ = await eventsAdderService.AddEvent(event3);
                List<EventResponse>? events_FromGetEvents = await _eventsGetterService.GetEvents();

                //Assert
                Assert.True(events_FromGetEvents.Count == 3);
            }
        }
        [Fact]
        public async void GetFilteredEvents_ToReturnFilteredEvents()
        {
            //Arrange
            Guid UserID = Guid.NewGuid();
            DateTime dateNow = DateTime.Now;
            EventAddRequest activeEvent1 = new EventAddRequest()
            {
                Title = "SampleTest",
                Description = "SampleTest",
                Type = "SampleTest",
                ThemeColor = EventColorOptions.orange.ToString(),
                isActive = true,
                StartDate = dateNow,
            };
            EventAddRequest InActiveEvent1 = new EventAddRequest()
            {
                Title = "SampleTest",
                Description = "SampleTest",
                Type = "SampleTest",
                ThemeColor = EventColorOptions.orange.ToString(),
                isActive = false,
                StartDate = dateNow,
            };
            EventAddRequest activeEvent2 = new EventAddRequest()
            {
                Title = "SampleTest",
                Description = "SampleTest",
                Type = "SampleTest",
                ThemeColor = EventColorOptions.orange.ToString(),
                isActive = true,
                StartDate = dateNow,
            };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                bool isCreated = context.Database.EnsureCreated();

                IEventsAdderRepository eventsAdderRepository = new EventsAdderRepository(context);
                IEventsGetterRepository eventsGetterRepository = new EventsGetterRepository(context);
                IEventsStatusUpdaterRepository eventsStatusUpdaterRepository = new EventsStatusUpdaterRepository(context);
                IEventsAdderService eventsAdderService = new EventsAdderService(eventsAdderRepository, eventsStatusUpdaterRepository, _mockSignedInUserService.Object);
                IEventsGetterService _eventsGetterService = new EventsGetterService(eventsGetterRepository, eventsStatusUpdaterRepository, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserID);
                //Act
                _ = await eventsAdderService.AddEvent(activeEvent1);
                _ = await eventsAdderService.AddEvent(InActiveEvent1);
                _ = await eventsAdderService.AddEvent(activeEvent2);
                List<EventResponse>? activeEvents_FromGetFilteredEvents = await _eventsGetterService.GetFilteredEvents(StatusType.Active);
                List<EventResponse>? inactiveEvents_FromGetFilteredEvents = await _eventsGetterService.GetFilteredEvents(StatusType.Completed);

                //Assert
                Assert.True(activeEvents_FromGetFilteredEvents.Count == 2);
                Assert.True(inactiveEvents_FromGetFilteredEvents.Count == 1);
            }
        }
    }
}

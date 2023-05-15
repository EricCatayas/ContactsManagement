using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.EventsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.DTO.EventsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.EventsManager;
using ContactsManagement.Core.Services.EventsManager;
using ContactsManagement.Infrastructure.DbContexts;
using ContactsManagement.Infrastructure.Repositories.EventsManager;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace ContactsManagement.UnitTests.Events
{
    public class EventsServiceTest
    {
        private readonly IEventsAdderService _eventsAdderService;
        private readonly IEventsGetterService _eventsGetterService;
        private readonly IEventsDeleterService _eventsDeleterService;
        private readonly IEventsUpdaterService _eventsUpdaterService;
        private readonly IEventsSorterService _eventsSorterService;

        private readonly Mock<IEventsAdderRepository> _eventsAdderRepositoryMock;
        private readonly Mock<IEventsGetterRepository> _eventsGetterRepositoryMock;
        private readonly Mock<IEventsUpdaterRepository> _eventsUpdaterRepositoryMock;
        private readonly Mock<IEventsStatusUpdaterRepository> _eventsStatusUpdaterRepositoryMock;
        private readonly Mock<IEventsDeleterRepository> _eventsDeleterRepositoryMock;
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;

        public EventsServiceTest()
        {
            _eventsAdderRepositoryMock = new Mock<IEventsAdderRepository>();
            _eventsGetterRepositoryMock = new Mock<IEventsGetterRepository>();
            _eventsUpdaterRepositoryMock = new Mock<IEventsUpdaterRepository>();
            _eventsStatusUpdaterRepositoryMock = new Mock<IEventsStatusUpdaterRepository>();
            _eventsDeleterRepositoryMock = new Mock<IEventsDeleterRepository>();
            _mockSignedInUserService = new Mock<ISignedInUserService>();

            _eventsAdderService = new EventsAdderService(_eventsAdderRepositoryMock.Object, _eventsStatusUpdaterRepositoryMock.Object, _mockSignedInUserService.Object);
            _eventsGetterService = new EventsGetterService(_eventsGetterRepositoryMock.Object, _eventsStatusUpdaterRepositoryMock.Object, _mockSignedInUserService.Object);
            _eventsDeleterService = new EventsDeleterService(_eventsDeleterRepositoryMock.Object, _mockSignedInUserService.Object);
            _eventsUpdaterService = new EventsUpdaterService(_eventsUpdaterRepositoryMock.Object, _eventsGetterRepositoryMock.Object, _mockSignedInUserService.Object);
            _eventsSorterService = new EventsSorterService();
        }

        #region AddEvent
        [Fact]
        public void AddEvent_NullArgument_ToThrowArgumentNullException()
        {
            //Arrange
            Guid UserID = Guid.NewGuid();
            EventAddRequest eventAddRequest = new EventAddRequest()
            {
                Title = "SampleTest",
                Description = "SampleTest",
                Type = "SampleTest",
                StartDate = DateTime.Now,
            };
            Event eventFromAddRequest = eventAddRequest.ToEvent();
            _eventsAdderRepositoryMock.Setup(temp => temp.AddEvent(It.IsAny<Event>())).ReturnsAsync(eventFromAddRequest);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserID);

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _eventsAdderService.AddEvent(eventAddRequest);
            });
        }
        [Fact]
        public async void AddEvent_ValidEventAddRequest_ToBeSuccessful()
        {
            //Arrange
            Guid UserID = Guid.NewGuid();
            DateTime dateNow = DateTime.Now;
            EventAddRequest eventAddRequest = new EventAddRequest()
            {
                Title = "SampleTest",
                Description = "SampleTest",
                Type = "SampleTest",
                StartDate = dateNow,
            };
            Event eventFromAddRequest = new()
            {
                Title = eventAddRequest.Title,
                Description = eventAddRequest.Description,
                Type = eventAddRequest.Type,
                StartDate = dateNow,
            };
            _eventsAdderRepositoryMock.Setup(temp => temp.AddEvent(It.IsAny<Event>())).ReturnsAsync(eventFromAddRequest);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserID);

            //Act
            EventResponse eventResponse = await _eventsAdderService.AddEvent(eventAddRequest);

            //Assert
            Assert.Equal(eventAddRequest.Title, eventResponse.Title);
            Assert.Equal(eventAddRequest.Description, eventResponse.Description);
            Assert.Equal(eventAddRequest.Type, eventResponse.Type);
            Assert.Equal(eventAddRequest.StartDate, eventResponse.StartDate);
        }
        [Fact]
        public async void AddEvent_NonSignedInUser_ToThrowAccessDeniedException()
        {
            //Arrange
            Guid UserID = Guid.NewGuid();
            DateTime dateNow = DateTime.Now;
            EventAddRequest eventAddRequest = new EventAddRequest()
            {
                Title = "SampleTest",
                Description = "SampleTest",
                Type = "SampleTest",
                StartDate = dateNow,
            };
            Event eventFromAddRequest = new()
            {
                Title = eventAddRequest.Title,
                Description = eventAddRequest.Description,
                Type = eventAddRequest.Type,
                StartDate = dateNow,
            };
            _eventsAdderRepositoryMock.Setup(temp => temp.AddEvent(It.IsAny<Event>())).ReturnsAsync(eventFromAddRequest);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(null as Guid?);

            //Act
            Func<Task> action = async () =>
            {
                _ = await _eventsAdderService.AddEvent(eventAddRequest);
            };

            //Assert
            await Assert.ThrowsAsync<AccessDeniedException>(action);
        }
        #endregion

        #region EventsGetterService
        [Fact]
        public async void GetEventById_NonExistentId_ToReturnNull()
        {
            //Arrange
            Guid UserID = Guid.NewGuid();
            Event? nullEvent = null;
            //Act
            _eventsGetterRepositoryMock.Setup(temp => temp.GetEvent(It.IsAny<int>(), It.IsAny<Guid>())).ReturnsAsync(nullEvent); 
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserID);

            EventResponse? eventResponse = await _eventsGetterService.GetEventById(1111);

            //Assert
            Assert.Null(eventResponse);
        }
        [Fact]
        public async void GetEventById_ValidID_ToBeSuccessful()
        {
            //Arrange
            Guid UserID = Guid.NewGuid();
            int EventID = 1000;
            Event? @event = new Event()
            {
                Title = "SampleTest",
                EventId = EventID,
                UserId = UserID,
                isActive = true,
            };

            //Act
            _eventsGetterRepositoryMock.Setup(temp => temp.GetEvent(It.IsAny<int>(), It.IsAny<Guid>())).ReturnsAsync(@event);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserID);

            //Assert
            EventResponse? eventResponse = await _eventsGetterService.GetEventById(EventID);
            Assert.True(eventResponse.Title == @event.Title && eventResponse.EventId == EventID);
        }
        [Fact]
        public async void GetEventById_NonSignedInUser_ToThrowAccessDeniedException()
        {
            //Arrange
            Guid UserID = Guid.NewGuid();
            int EventID = 1000;
            Event? @event = new Event()
            {
                Title = "SampleTest",
                EventId = EventID,
                UserId = UserID,
                isActive = true,
            };

            //Act
            _eventsGetterRepositoryMock.Setup(temp => temp.GetEvent(It.IsAny<int>(), It.IsAny<Guid>())).ReturnsAsync(@event);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(null as Guid?);

            Func<Task> action = async () =>
            {
                _ = await _eventsGetterService.GetEventById(EventID);
            };
            //Assert
            Assert.ThrowsAsync<AccessDeniedException>(() => action());  
        }
        #endregion


        #region GetAllEvents
        [Fact]
        public async void GetEvents_NoData_ToReturnNull()
        {
            //Arrange
            Guid UserID = Guid.NewGuid();
            int EventID = 1000;
            List<Event>? events_Null = null;
            _eventsGetterRepositoryMock.Setup(temp => temp.GetEvents(It.IsAny<Guid>())).ReturnsAsync(events_Null);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserID);
            _eventsStatusUpdaterRepositoryMock.Setup(temp => temp.UpdateEventsStatus(It.IsAny<List<Event>>())).ReturnsAsync(events_Null);
            //Act
            List<EventResponse>? events_ToBeNull = await _eventsGetterService.GetEvents();

            //Assert
            Assert.Null(events_ToBeNull);
        }
        [Fact]
        public async void GetEvents_NonSignedInUser_ToThrowAccessDeniedException()
        {
            //Arrange
            List<Event>? events_Null = null;
            _eventsGetterRepositoryMock.Setup(temp => temp.GetEvents(It.IsAny<Guid>())).ReturnsAsync(events_Null);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(null as Guid?);
            _eventsStatusUpdaterRepositoryMock.Setup(temp => temp.UpdateEventsStatus(It.IsAny<List<Event>>())).ReturnsAsync(events_Null);

            //Act
            Func<Task> action = async () =>
            {
                _ = await _eventsGetterService.GetEvents();
            };

            //Assert
            Assert.ThrowsAsync<AccessDeniedException>(() => action());
        }
        [Fact]
        public async void GetEvents_ContainingData_ToBeSuccessful()
        {
            //Arrange
            Guid UserID = Guid.NewGuid();
            int EventID = 1000;
            List<Event> events = new List<Event>()
            {
                new Event()
                {
                    EventId = 1000,
                    Title = "Sample",
                    isActive = true,
                    UserId = UserID,
                },
                new Event()
                {
                    EventId = 1001,
                    Title = "Sample",
                    isActive = true,
                    UserId = UserID,
                },
                new Event()
                {
                    EventId = 1003,
                    Title = "Sample",
                    isActive = true,
                    UserId = UserID,
                },
            };
            _eventsGetterRepositoryMock.Setup(temp => temp.GetEvents(It.IsAny<Guid>())).ReturnsAsync(events);
            _eventsStatusUpdaterRepositoryMock.Setup(temp => temp.UpdateEventsStatus(It.IsAny<List<Event>>())).ReturnsAsync(events);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserID);
            //Act
            List<EventResponse>? events_FromGetEvents = await _eventsGetterService.GetEvents();

            //Assert
            Assert.NotNull(events_FromGetEvents);
            Assert.NotEmpty(events_FromGetEvents);
        }
        #endregion


        #region GetFilteredEvents
        [Fact]
        public async void GetFilteredEvents_ActiveEvents_ToBeSuccessful()
        {
            //Arrange
            Guid UserID = Guid.NewGuid();
            List<Event> ActiveEvents = new List<Event>()
            {
                new Event()
                {
                    UserId = UserID,
                    EventId = 1000,
                    Title = "Sample",
                    Description = "Sample",
                    Status = "Sample",
                    isActive = true,
                    LastUpdatedDate= DateTime.Now,
                },
                new Event()
                {
                    UserId = UserID,
                    EventId = 1001,
                    Title = "Sample",
                    Description = "Sample",
                    Status = "Sample",
                    isActive = true,
                    LastUpdatedDate= DateTime.Now,
                },
                new Event()
                {
                    UserId = UserID,
                    EventId = 1002,
                    Title = "Sample",
                    Description = "Sample",
                    Status = "Sample",
                    isActive = true,
                    LastUpdatedDate= DateTime.Now,
                }
            };
            _eventsGetterRepositoryMock.Setup(temp => temp.GetFilteredEvents(It.IsAny<Expression<Func<Event, bool>>>(), It.IsAny<Guid>())).ReturnsAsync(ActiveEvents);
            _eventsStatusUpdaterRepositoryMock.Setup(temp => temp.UpdateEventsStatus(It.IsAny<List<Event>>())).ReturnsAsync(ActiveEvents);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserID);
            //Act
            List<EventResponse>? eventResponses = await _eventsGetterService.GetFilteredEvents(StatusType.Active);

            //Assert
            eventResponses.ForEach((@event) =>
            {
                Assert.True(@event.isActive);
            });
        }
        [Fact]
        public async void GetFilteredEvents_InActiveEvents_ToBeSuccessful()
        {
            //Arrange
            Guid UserID = Guid.NewGuid();
            List<Event> InActiveEvents = new List<Event>()
            {
                new Event()
                {
                    UserId = UserID,
                    EventId = 1000,
                    Title = "Sample",
                    Description = "Sample",
                    Status = "Sample",
                    isActive = false,
                    LastUpdatedDate= DateTime.Now,
                },
                new Event()
                {
                    UserId = UserID,
                    EventId = 1001,
                    Title = "Sample",
                    Description = "Sample",
                    Status = "Sample",
                    isActive = false,
                    LastUpdatedDate= DateTime.Now,
                },
                new Event()
                {
                    UserId = UserID,
                    EventId = 1002,
                    Title = "Sample",
                    Description = "Sample",
                    Status = "Sample",
                    isActive = false,
                    LastUpdatedDate= DateTime.Now,
                }
            };
            _eventsGetterRepositoryMock.Setup(temp => temp.GetFilteredEvents(It.IsAny<Expression<Func<Event, bool>>>(), It.IsAny<Guid>())).ReturnsAsync(InActiveEvents);
            _eventsStatusUpdaterRepositoryMock.Setup(temp => temp.UpdateEventsStatus(It.IsAny<List<Event>>())).ReturnsAsync(InActiveEvents);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserID);
            //Act
            List<EventResponse>? eventResponses = await _eventsGetterService.GetFilteredEvents(StatusType.Active);

            //Assert
            eventResponses.ForEach((@event) =>
            {
                Assert.False(@event.isActive);
            });
        }
        [Fact]
        public async void GetFilteredEvents_NonSignedInUser_ToThrowAccessDeniedException()
        {
            //Arrange
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(null as Guid?);

            //Act
            Func<Task> action = async () =>
            {
                _ = await _eventsGetterService.GetFilteredEvents(StatusType.Active);
            };

            //Assert
            Assert.ThrowsAsync<AccessDeniedException>(() => action());
        }
        #endregion


        #region UpdateEvent
        [Fact]
        public async void UpdateEvent_NonSignedInUser_ToThrowAccessDeniedException()
        {
            EventUpdateRequest eventUpdateRequest = new()
            {
                EventId = 1000,
                Title = "Sample",
                ThemeColor = "Sample",
                Description = "Sample",
            };
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(null as Guid?);
            Func<Task> action = async () =>
            {
                await _eventsUpdaterService.UpdateEvent(eventUpdateRequest);
            };
            await Assert.ThrowsAsync<AccessDeniedException>(() => action());
        }
        #endregion
    }
}

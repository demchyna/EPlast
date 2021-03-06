﻿using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Services.EventUser;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Services.Events
{
    public class EventUserManagerTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IEventCategoryManager> _eventCategoryManager;
        private readonly Mock<IEventStatusManager> _eventStatusManager;
        private readonly Mock<IEventAdministrationTypeManager> _eventAdministrationTypeManager;

        public EventUserManagerTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _eventCategoryManager = new Mock<IEventCategoryManager>();
            _eventStatusManager = new Mock<IEventStatusManager>();
            _eventAdministrationTypeManager = new Mock<IEventAdministrationTypeManager>();
        }

        [Fact]
        public async Task InitializeEventCreateDTOTest()
        {
            //Arrange
            _eventCategoryManager.Setup(x => x.GetDTOAsync());
            _mapper.Setup(m => m.Map<List<User>, IEnumerable<UserDTO>>(It.IsAny<List<User>>()))
                .Returns(new List<UserDTO>());
            _mapper.Setup(a => a.Map<List<EventType>, IEnumerable<EventTypeDTO>>(It.IsAny<List<EventType>>()))
                .Returns(new List<EventTypeDTO>());
            _repoWrapper.Setup(x => x.User.GetFirstAsync(It.IsAny<Expression<Func<User, bool>>>(), null))
                .ReturnsAsync(new User());
            _repoWrapper.Setup(e => e.EventType.GetFirstAsync(It.IsAny<Expression<Func<EventType, bool>>>(), null))
                .ReturnsAsync(new EventType());

            //Act
            var eventUserManager = new EventUserManager(_repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object,
                _eventStatusManager.Object, _eventAdministrationTypeManager.Object);
            var methodResult = await eventUserManager.InitializeEventCreateDTOAsync();

            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<EventCreateDTO>(methodResult);
        }

        [Fact]
        public async Task CreateEventTest()
        {
            int statusId = 1;
            _eventStatusManager.Setup(s => s.GetStatusIdAsync(It.IsAny<string>())).ReturnsAsync(statusId);

            _mapper.Setup(m => m.Map<EventCreationDTO, Event>(It.IsAny<EventCreationDTO>()))
                .Returns(new Event());
            _repoWrapper.Setup(r => r.EventAdmin.CreateAsync(It.IsAny<EventAdmin>()));
            _repoWrapper.Setup(r => r.EventAdministration.CreateAsync(It.IsAny<EventAdministration>()));
            _repoWrapper.Setup(r => r.Event.CreateAsync(It.IsAny<Event>()));

            //Act
            var eventUserManager = new EventUserManager(_repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object,
                _eventStatusManager.Object, _eventAdministrationTypeManager.Object);
            var methodResult = await eventUserManager.CreateEventAsync(GetEventCreateDTO());

            //Assert
            Assert.IsType<int>(methodResult);
        }

        [Fact]
        public async Task InitializeEventEditDTOTest()
        {
            //Arrange
            int eventId = 1;

            _repoWrapper.Setup(r => r.Event.GetFirstAsync(It.IsAny<Expression<Func<Event, bool>>>(), null))
                .ReturnsAsync(new Event());
            _repoWrapper.Setup(r => r.EventType.GetFirstAsync(It.IsAny<Expression<Func<EventType, bool>>>(), null))
                .ReturnsAsync(new EventType());
            _repoWrapper.Setup(x => x.User.GetFirstAsync(It.IsAny<Expression<Func<User, bool>>>(), null))
                .ReturnsAsync(new User());
            _eventCategoryManager.Setup(x => x.GetDTOAsync());
            _repoWrapper.Setup(x => x.EventAdministration.GetFirstAsync(It.IsAny<Expression<Func<EventAdministration, bool>>>(), null))
                .ReturnsAsync(new EventAdministration());

            var eventUserManager = new EventUserManager(_repoWrapper.Object, _mapper.Object, _eventCategoryManager.Object,
                _eventStatusManager.Object, _eventAdministrationTypeManager.Object);
            var methodResult = await eventUserManager.InitializeEventEditDTOAsync(eventId);

            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<EventCreateDTO>(methodResult);
        }

        public EventCreateDTO GetEventCreateDTO()
        {
            var eventCreate = new EventCreateDTO
            {
                Event = new EventCreationDTO { },
                Сommandant = new EventAdministrationDTO { },
                Alternate = new EventAdministrationDTO { },
                Bunchuzhnyi = new EventAdministrationDTO { },
                Pysar = new EventAdministrationDTO { },
                EventCategories = new List<EventCategoryDTO>
                {
                    new EventCategoryDTO { }
                },
                EventTypes = new List<EventTypeDTO>
                {
                    new EventTypeDTO { }
                },
                Users = new List<UserInfoDTO>
                {
                    new UserInfoDTO { }
                }
            };
            return eventCreate;
        }
    }
}
﻿using EPlast.BLL.DTO.Notification;
using EPlast.BLL.Interfaces.Notifications;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Controllers;
using EPlast.WebApi.WebSocketHandlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    class NotificationBoxControllerTests
    {
        private Mock<UserNotificationHandler> _userNotificationHandler;
        private Mock<INotificationService> _notificationService;

        private NotificationBoxController _notificationBoxController =>
        new NotificationBoxController(_notificationService.Object, _userNotificationHandler.Object);

        public NotificationBoxControllerTests()
        {
            _notificationService = new Mock<INotificationService>();
            Mock<INotificationConnectionManager> connectionManager = new Mock<INotificationConnectionManager>();
            _userNotificationHandler = new Mock<UserNotificationHandler>(connectionManager.Object);
        }


        [Test]
        public async Task GetAllTypes_Valid_Test()
        {
            //Arrange
            _notificationService.Setup(cs => cs.GetAllNotificationTypesAsync()).ReturnsAsync(new List<NotificationTypeDTO>());

            NotificationBoxController notificationBoxController = _notificationBoxController;

            //Act
            var result = await notificationBoxController.GetAllTypes();
            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(result);
        }

        [TestCase("2")]
        public async Task GetAllUserNotification_Valid_Test(string id)
        {
            //Arrange
            _notificationService.Setup(cs => cs.GetAllUserNotificationsAsync(It.IsAny<string>())).ReturnsAsync(new List<UserNotificationDTO>());

            NotificationBoxController notificationBoxController = _notificationBoxController;

            //Act
            var result = await notificationBoxController.GetAllUserNotification(id);
            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.NotNull(result);
        }

        [TestCase(2)]
        public async Task RemoveUserNotification_Valid_Test(int id)
        {
            //Arrange
            bool successRemoved = true;
            _notificationService.Setup(cs => cs.RemoveUserNotificationAsync(It.IsAny<int>())).ReturnsAsync(successRemoved);

            NotificationBoxController notificationBoxController = _notificationBoxController;

            //Act
            var result = await notificationBoxController.RemoveUserNotification(id);
            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            
        }

        [TestCase(2)]
        public async Task RemoveUserNotification_InValid_Test(int id)
        {
            //Arrange
            bool successRemoved = false;
            _notificationService.Setup(cs => cs.RemoveUserNotificationAsync(It.IsAny<int>())).ReturnsAsync(successRemoved);

            NotificationBoxController notificationBoxController = _notificationBoxController;

            //Act
            var result = await notificationBoxController.RemoveUserNotification(id);
            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);

        }

        [TestCase("2")]
        public async Task RemoveAllUserNotifications_Valid_Test(string id)
        {
            //Arrange
            bool successRemoved = true;
            _notificationService.Setup(cs => cs.RemoveAllUserNotificationAsync(It.IsAny<string>())).ReturnsAsync(successRemoved);

            NotificationBoxController notificationBoxController = _notificationBoxController;

            //Act
            var result = await notificationBoxController.RemoveAllUserNotifications(id);
            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);

        }

        [TestCase("2")]
        public async Task RemoveAllUserNotifications_InValid_Test(string id)
        {
            //Arrange
            bool successRemoved = false;
            _notificationService.Setup(cs => cs.RemoveAllUserNotificationAsync(It.IsAny<string>())).ReturnsAsync(successRemoved);

            NotificationBoxController notificationBoxController = _notificationBoxController;

            //Act
            var result = await notificationBoxController.RemoveAllUserNotifications(id);
            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);

        }

        [Test]
        public async Task SetCheckForListNotification_Valid_Test()
        {
            //Arrange
            bool successSetCheck = true;
            _notificationService.Setup(cs => cs.SetCheckForListNotificationAsync(It.IsAny<string>())).ReturnsAsync(successSetCheck);

            NotificationBoxController notificationBoxController = _notificationBoxController;

            //Act
            var result = await notificationBoxController.SetCheckForListNotification("2");
            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);

        }

        [Test]
        public async Task SetCheckForListNotification_InValid_Test()
        {
            //Arrange
            bool successSetCheck = false;
            _notificationService.Setup(cs => cs.SetCheckForListNotificationAsync(It.IsAny<string>())).ReturnsAsync(successSetCheck);

            NotificationBoxController notificationBoxController = _notificationBoxController;

            //Act
            var result = await notificationBoxController.SetCheckForListNotification("2");
            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);

        }

        [Test]
        public async Task AddNotificationList_InValid_ThrowException()
        {
            //Arrange
            _notificationService.Setup(cs => cs.AddListUserNotificationAsync(It.IsAny<IEnumerable<UserNotificationDTO>>())).ThrowsAsync(new InvalidOperationException()); 

            NotificationBoxController notificationBoxController = _notificationBoxController;

            //Act
            var result = await notificationBoxController.AddNotificationList(new List<UserNotificationDTO>());
            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task AddNotificationList_Valid_ReturnsNoContent()
        {
            //Arrange
            _notificationService.Setup(cs => cs.AddListUserNotificationAsync(It.IsAny<IEnumerable<UserNotificationDTO>>())).ReturnsAsync(new List<UserNotificationDTO>());


            NotificationBoxController notificationBoxController = _notificationBoxController;

            //Act
            var result = await notificationBoxController.AddNotificationList(new List<UserNotificationDTO>());
            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
           
        }

    }
}

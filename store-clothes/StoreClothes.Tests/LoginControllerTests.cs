
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using store_clothes.Controllers;
using store_clothes.Models;
using MockQueryable.Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;


namespace StoreClothes.Tests
{
    public class LoginControllerTests
    {
        private LoginController GetControllerWithContext(List<User> fakeUsers)
        {
            var userDbSet = fakeUsers.AsQueryable().BuildMockDbSet();


            var mockContext = new Mock<storeclothesContext>();
            mockContext.Setup(c => c.Users).Returns(userDbSet.Object);

            var controller = new LoginController(mockContext.Object);

            var httpContext = new DefaultHttpContext();
            httpContext.Session = new MockHttpSession();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            return controller;
        }


        [Fact]
        public void Authenticate_ValidUser_RedirectsToHome()
        {
            // Arrange
            var fakeUsers = new List<User>
            {
                new User { Id = 1, Email = "test@example.com", Password = "123", Name = "Test User" }
            };
            var controller = GetControllerWithContext(fakeUsers);

            // Act
            var result = controller.Authenticate("test@example.com", "123");

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Home", redirect.ControllerName);
        }

        [Fact]
        public void Authenticate_InvalidUser_ReturnsLoginViewWithError()
        {
            // Arrange
            var fakeUsers = new List<User>(); // Không có user nào
            var controller = GetControllerWithContext(fakeUsers);

            // Act
            var result = controller.Authenticate("wrong@example.com", "wrong");

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", view.ViewName);
            Assert.NotNull(controller.ViewBag.Error);
        }
    }

    // Tạo mock session
    public class MockHttpSession : ISession
    {
        private Dictionary<string, byte[]> _sessionStorage = new Dictionary<string, byte[]>();

        public bool IsAvailable => true;
        public string Id => "mock_session";
        public IEnumerable<string> Keys => _sessionStorage.Keys;

        public void Clear() => _sessionStorage.Clear();
        public void Remove(string key) => _sessionStorage.Remove(key);

        public void Set(string key, byte[] value) => _sessionStorage[key] = value;

        public bool TryGetValue(string key, out byte[] value) => _sessionStorage.TryGetValue(key, out value);

        public void SetString(string key, string value) => Set(key, System.Text.Encoding.UTF8.GetBytes(value));
        public string GetString(string key)
        {
            return _sessionStorage.TryGetValue(key, out var value) ? System.Text.Encoding.UTF8.GetString(value) : null;
        }

        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    }
}

using Microsoft.AspNetCore.Identity;
using ProductCatalog.Core.Entities;
using Moq;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProductCatalog.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Web.Models.Account;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ProductCatalog.Tests.WepLayerTest
{
    public class AccountControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
        private readonly Mock<ILogger<AccountController>> _mockLogger;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            // Mock IUserStore<ApplicationUser> (required for UserManager)
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();


            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);


            _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                _mockUserManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null);


            _mockLogger = new Mock<ILogger<AccountController>>();

            // Initialize the controller with mocked dependencies
            _controller = new AccountController(
                _mockUserManager.Object,
                _mockSignInManager.Object,
                _mockLogger.Object
                );
        }



        [Fact]
        public async Task Login_ShouldRedirectToProductsIndex_WhenLoginIsSuccessful()
        {
            // Arrange
            var model = new LoginViewModel
            {

                Email = "Email@example.com",
                Password = "Password",
                RememberMe = false
            };

            _mockSignInManager
                .Setup(m => m.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false))
                .ReturnsAsync(SignInResult.Success);


            //Act
            var result = await _controller.Login(model);

            //assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Products", redirectResult.ControllerName);
            Assert.Equal("Index", redirectResult.ActionName);


        }


        [Fact]
        public async Task Login_ShouldReturnView_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Invalid model state");

            // Act
            var result = await _controller.Login(new LoginViewModel());

            // Assert
            Assert.IsType<ViewResult>(result);
        }



        [Fact]
        public async Task Register_ShouldRedirectToProductsIndex_WhenRegisterIsSuccessful()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                Email = "Email",
                Password = "Password",
                FullName = "FullName",
              
            };
            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), model.Password))
                .ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), "User"))
                .ReturnsAsync(IdentityResult.Success);
            _mockSignInManager.Setup(m => m.SignInAsync(It.IsAny<ApplicationUser>(), false, null))
                .Returns(Task.FromResult((object)null));

            //Act
            var result = await _controller.Register(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Products", redirectResult.ControllerName);
            Assert.Equal("Index", redirectResult.ActionName);   

        }


        [Fact]
        public async Task Register_ShouldReturnView_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Invalid model state");
            // Act
            var result = await _controller.Register(new RegisterViewModel());
            // Assert
            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public async Task Lodout_RedirectToIndex_WhenSuccess()
        {
            // Arrange
            _mockSignInManager.Setup(m => m.SignOutAsync()).Returns(Task.CompletedTask);
            // Act
            var result = await _controller.Logout();
            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Products", redirectResult.ControllerName);
            Assert.Equal("Index", redirectResult.ActionName);
        }
    }
}



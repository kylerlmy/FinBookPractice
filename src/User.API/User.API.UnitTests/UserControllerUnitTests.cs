using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.API.Controllers;
using User.API.Data;
using User.API.Models;
using Xunit;

namespace User.API.UnitTests
{
    public class UserControllerUnitTests
    { 
        private UserContext GetUserContext()
        {
            var options = new DbContextOptionsBuilder<UserContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;

            var userContext = new UserContext(options);

            userContext.Users.Add(new AppUser { Id = 1, Name = "kyle" });
           userContext.UserProperties.Add(new UserProperty { Key = "fin_Company", Value = "百度", Text = "百度" });

            userContext.SaveChanges();

            return userContext;
        }

        private UserController GetUserController1()
        {
            var context = GetUserContext();

            var loggerMock = new Mock<ILogger<UserController>>();
            var logger = loggerMock.Object;

            return new UserController(context, logger);
        }

        private (UserController userController, UserContext userContext) GetUserController()
        {
            var context = GetUserContext();

            var loggerMock = new Mock<ILogger<UserController>>();
            var logger = loggerMock.Object;

            return (userController: new UserController(context, logger), userContext: context);
        }


        [Fact]
        public async Task Get_ReturnRightUser_WithExceptedParameters()
        {
            // var controller = GetUserController();

            (UserController controller, UserContext context) = GetUserController();
            var response = await controller.Get();

            // Assert.IsType<JsonResult>(response);

            var result = response.Should().BeOfType<JsonResult>().Subject;
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;

            appUser.Id.Should().Be(1);
            appUser.Name.Should().Be("kyle");

        }

        [Fact]
        public async Task Get_ReturnNewName_WithExceptedNewNameParameters()
        {
            //var controller = GetUserController();
            (UserController controller, UserContext context) = GetUserController();
            var document = new JsonPatchDocument<AppUser>();
            document.Replace(u => u.Name, "yuan");
            var response = await controller.Patch(document);
            var result = response.Should().BeOfType<JsonResult>().Subject;

            //assert response
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Name.Should().Be("yuan");

            //assert name value in ef context
            var userModel = await context.Users.SingleOrDefaultAsync(u => u.Id == 1);
            userModel.Should().NotBeNull();
            userModel.Name.Should().Be("yuan");

        }

        [Fact]
        public async Task Get_ReturnNewProperties_WithAddNewProperty()
        {
            //var controller = GetUserController();
            (UserController controller, UserContext context) = GetUserController();
            var document = new JsonPatchDocument<AppUser>();
            document.Replace(u => u.Properties, new List<UserProperty> {
                new UserProperty{ Key="fin_industry",Value="腾讯",Text="腾讯"}
            });

            var response = await controller.Patch(document);
            var result = response.Should().BeOfType<JsonResult>().Subject;

            //assert response
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Properties.Count.Should().Be(1);
            appUser.Properties.First().Value.Should().Be("腾讯");
            appUser.Properties.First().Key.Should().Be("fin_industry");

            //assert name value in ef context
            var userModel = await context.Users.SingleOrDefaultAsync(u => u.Id == 1);
            userModel.Properties.Count.Should().Be(1);
            userModel.Properties.First().Value.Should().Be("腾讯");
            userModel.Properties.First().Key.Should().Be("fin_industry");
        }


        [Fact]
        public async Task Get_ReturnNewProperties_WithRemoveProperty()
        {
            //var controller = GetUserController();
            (UserController controller, UserContext context) = GetUserController();
            var document = new JsonPatchDocument<AppUser>();
            document.Replace(u => u.Properties, new List<UserProperty>
            {
            });

            var response = await controller.Patch(document);
            var result = response.Should().BeOfType<JsonResult>().Subject;

            //assert response
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Properties.Should().BeEmpty();

            //assert name value in ef context
            var userModel = await context.Users.SingleOrDefaultAsync(u => u.Id == 1);
            userModel.Properties.Should().BeEmpty();
        }
    }
}

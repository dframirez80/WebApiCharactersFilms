using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using WebApiCharactersFilms.Controllers;
using WebApiCharactersFilms.Handlers;
using WebApiCharactersFilms.Mail;
using WebApiCharactersFilms.Repository.IRepositories;
using WebApiCharactersFilms.Repository.Repositories;
using WebApiCharactersFilms.Security;
using WebApiCharactersFilms.Constants;
using Xunit;
using WebApiCharactersFilms.Models.Dtos;
using System.Collections.Generic;
using WebApiCharactersFilms.Models;

namespace WebApiCharactersFilms.Test
{
    public class AuthControllerTest
    {
        public Mock<IUsersHandler> _handler = new();

        //----------------------------------------------------------------------
        // Get
        [Fact]
        public async Task GetConfirmAsync_Should_Return_Action_BadRequest() {
            // arrange
            int id = 1;
            string myToken = "my token";
            _handler.Setup(a => a.ConfirmUserAsync(id, myToken)).ReturnsAsync(Constants.Mail.ConfirmFail);
            var expected = StatusCodes.Status400BadRequest;
            var controller = new AuthController(_handler.Object);
            // act
            var result = await controller.GetConfirmAsync(id, myToken);
            // assert
            var resp = result as ObjectResult;
            Assert.Equal(expected, resp.StatusCode);
        }

        [Fact]
        public async Task GetConfirmAsync_Should_Return_Action_Ok() {
            // arrange
            int id = 1;
            string myToken = "my token";
            _handler.Setup(a => a.ConfirmUserAsync(id, myToken)).ReturnsAsync(Constants.Mail.Confirm);
            var expected = StatusCodes.Status200OK;
            var controller = new AuthController(_handler.Object);
            // act
            var result = await controller.GetConfirmAsync(id, myToken);
            // assert
            var resp = result as ObjectResult;
            Assert.Equal(expected, resp.StatusCode);
        }

        //----------------------------------------------------------------------
        // Post
        
        [Fact]
        public async Task PostChangePasswordAsync_Should_Return_Action_BadRequest() {
            // arrange
            var pass = new ChangePassword() { NewPassword = "xxx@cmail.com" , OldPassword = "xyyxx@cmail.com" };
            _handler.Setup(a => a.ChangeUserPasswordAsync(pass)).ReturnsAsync(ErrorMessage.EmailOrPassword);
            var expected = StatusCodes.Status400BadRequest;
            var controller = new AuthController(_handler.Object);
            // act
            var result = await controller.PostChangePasswordAsync(pass);
            // assert
            var resp = result as ObjectResult;
            Assert.Equal(expected, resp.StatusCode);
        }
        [Fact]
        public async Task PostChangePasswordAsync_Should_Return_Action_Ok() {
            // arrange
            var pass = new ChangePassword() { NewPassword = "xxx@cmail.com", OldPassword = "xyyxx@cmail.com" };
            _handler.Setup(a => a.ChangeUserPasswordAsync(pass)).ReturnsAsync(Constants.Mail.ChangePassword);
            var expected = StatusCodes.Status200OK;
            var controller = new AuthController(_handler.Object);
            // act
            var result = await controller.PostChangePasswordAsync(pass);
            // assert
            var resp = result as ObjectResult;
            Assert.Equal(expected, resp.StatusCode);
        }

        [Fact]
        public async Task PostUserLoginAsync_Should_Return_Action_BadRequest() {
            // arrange
            var login = new Login();
            _handler.Setup(a => a.LoginUserAsync(login)).ReturnsAsync(ErrorMessage.UserNotLogin);
            var expected = StatusCodes.Status400BadRequest;
            var controller = new AuthController(_handler.Object);
            // act
            var result = await controller.PostUserLoginAsync(login);
            // assert
            var resp = result as ObjectResult;
            Assert.Equal(expected, resp.StatusCode);
        }

        [Fact]
        public async Task PostUserLoginAsync_Should_Return_Action_Ok() {
            // arrange
            var login = new Login();
            _handler.Setup(a => a.LoginUserAsync(login)).ReturnsAsync("token valido");
            var expected = StatusCodes.Status200OK;
            var controller = new AuthController(_handler.Object);
            // act
            var result = await controller.PostUserLoginAsync(login);
            // assert
            var resp = result as ObjectResult;
            Assert.Equal(expected, resp.StatusCode);
        }
        /*[Fact]
        public async Task PostRegisterAsync_Should_Return_Action_Ok() {
            // arrange
            var dto = new UserDto();
            _handler.Setup(a => a.RegisterUserAsync(dto, "hola")).ReturnsAsync("habilitado por email");
            var expected = StatusCodes.Status200OK;
            var controller = new AuthController(_handler.Object);
            // act
            var result = await controller.PostRegisterAsync(dto);
            // assert
            var resp = result as ObjectResult;
            Assert.Equal(expected, resp.StatusCode);
        }*/
    }
}

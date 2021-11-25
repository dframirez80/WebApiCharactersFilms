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

namespace WebApiCharactersFilms.Test
{
    public class MoviesControllerTest
    {
        public Mock<IFilmsHandler> _handler = new();

        //----------------------------------------------------------------------
        // delete
        [Fact]
        public async Task DeleteFilmAsync_Should_Return_Action_NoContent() {
            // arrange
            _handler.Setup(a => a.DeleteFilmAsync(1));
            var expected = StatusCodes.Status204NoContent;
            var controller = new MoviesController(_handler.Object);
            // act
            var result = await controller.DeleteFilmAsync(1);
            // assert
            var resp = (StatusCodeResult)result;
            Assert.Equal(expected, resp.StatusCode);
        }

        //----------------------------------------------------------------------
        // Get
        [Fact]
        public async Task GetCharacterAsync_Should_Return_Action_NotFound() {
            // arrange
            var detailDto = new FilmDetailDto();
            detailDto = null;
            _handler.Setup(a => a.GetFilmWithCharactersAsync(1)).ReturnsAsync(detailDto);
            var expected = StatusCodes.Status404NotFound;
            var controller = new MoviesController(_handler.Object);
            // act
            var result = await controller.GetFilmAsync(1);
            // assert
            var resp = (StatusCodeResult)result.Result;
            Assert.Equal(expected, resp.StatusCode);
        }

        [Fact]
        public async Task GetFilmAsync_Should_Return_Action_Ok() {
            // arrange
            var detailDto = new FilmDetailDto()
            {
                FilmId = 1
            };
            _handler.Setup(a => a.GetFilmWithCharactersAsync(1)).ReturnsAsync(detailDto);
            var expected = StatusCodes.Status200OK;
            var controller = new MoviesController(_handler.Object);
            // act
            var result = await controller.GetFilmAsync(1);
            // assert
            var resp = result.Result as OkObjectResult;
            Assert.Equal(expected, resp.StatusCode);
        }

        //----------------------------------------------------------------------
        // Post
        [Fact]
        public async Task PostFilmAsync_Should_Return_Action_Created() {
            // arrange
            var createDto = new FilmCreateDto()
            {
                Title = "Sherk",
                Characters = new List<string>() { "Eddie"}
            };
            var Id = 1;
            _handler.Setup(a => a.CreateFilmAsync(createDto)).ReturnsAsync(Id);
            var expected = StatusCodes.Status201Created;

            var controller = new MoviesController(_handler.Object);
            // act
            var result = await controller.PostFilmAsync(createDto);
            // assert
            var resp = result as CreatedResult;
            Assert.Equal(expected, resp.StatusCode);
        }
        [Fact]
        public async Task PostFilmAsync_Should_Return_Action_BadRequest() {
            // arrange
            var createDto = new FilmCreateDto();
            var Id = 0;
            _handler.Setup(a => a.CreateFilmAsync(createDto)).ReturnsAsync(Id);
            var expected = StatusCodes.Status400BadRequest;

            var controller = new MoviesController(_handler.Object);
            // act
            var result = await controller.PostFilmAsync(createDto);
            // assert
            var resp = result as ObjectResult;
            Assert.Equal(expected, resp.StatusCode);
        }

        //----------------------------------------------------------------------
        // Put
        [Fact]
        public async Task PutFilmAsync_Should_Return_Action_NoContent() {
            // arrange
            var createDto = new FilmUpdateDto();
            var responseDto = new FilmResponseDto();
            _handler.Setup(a => a.UpdateFilmAsync(createDto));
            _handler.Setup(b => b.GetFilmAsync(1)).ReturnsAsync(responseDto);
            var expected = StatusCodes.Status204NoContent;

            var controller = new MoviesController(_handler.Object);
            // act
            var result = await controller.PutFilmAsync(1, createDto);
            // assert
            var resp = result as NoContentResult;
            Assert.Equal(expected, resp.StatusCode);
        }

        [Fact]
        public async Task PutFilmAsync_Should_Return_Action_BadRequest() {
            // arrange
            var createDto = new FilmUpdateDto();
            _handler.Setup(a => a.UpdateFilmAsync(createDto));
            _handler.Setup(b => b.GetFilmAsync(1));
            var expected = StatusCodes.Status400BadRequest;

            var controller = new MoviesController(_handler.Object);
            // act
            var result = await controller.PutFilmAsync(1, createDto);
            // assert
            var resp = result as BadRequestResult;
            Assert.Equal(expected, resp.StatusCode);
        }
    }
}
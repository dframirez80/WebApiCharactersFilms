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
    public class GenresControllerTest
    {
        public Mock<IGenresHandler> _handler = new();

        //----------------------------------------------------------------------
        // delete
        [Fact]
        public async Task DeleteGenreAsync_Should_Return_Action_NoContent() {
            // arrange
            _handler.Setup(a => a.DeleteGenreAsync(1));
            var expected = StatusCodes.Status204NoContent;
            var controller = new GenresController(_handler.Object);
            // act
            var result = await controller.DeleteGenreAsync(1);
            // assert
            var resp = (StatusCodeResult)result;
            Assert.Equal(expected, resp.StatusCode);
        }

        //----------------------------------------------------------------------
        // Get
        [Fact]
        public async Task GetGenreAsync_Should_Return_Action_NotFound() {
            // arrange
            var detailDto = new GenreResponseDto();
            detailDto = null;
            _handler.Setup(a => a.GetGenreAsync(1)).ReturnsAsync(detailDto);
            var expected = StatusCodes.Status404NotFound;
            var controller = new GenresController(_handler.Object);
            // act
            var result = await controller.GetGenreAsync(1);
            // assert
            var resp = (StatusCodeResult)result.Result;
            Assert.Equal(expected, resp.StatusCode);
        }

        [Fact]
        public async Task GetGenreAsync_Should_Return_Action_Ok() {
            // arrange
            var detailDto = new GenreResponseDto()
            {
                GenreId = 1
            };
            _handler.Setup(a => a.GetGenreAsync(1)).ReturnsAsync(detailDto);
            var expected = StatusCodes.Status200OK;
            var controller = new GenresController(_handler.Object);
            // act
            var result = await controller.GetGenreAsync(1);
            // assert
            var resp = result.Result as OkObjectResult;
            Assert.Equal(expected, resp.StatusCode);
        }

        //----------------------------------------------------------------------
        // Post
        [Fact]
        public async Task PostGenreAsync_Should_Return_Action_Created() {
            // arrange
            var createDto = new GenreCreateDto()
            {
                Name = "Drama"
            };
            var Id = 1;
            _handler.Setup(a => a.CreateGenreAsync(createDto)).ReturnsAsync(Id);
            var expected = StatusCodes.Status201Created;

            var controller = new GenresController(_handler.Object);
            // act
            var result = await controller.PostGenreAsync(createDto);
            // assert
            var resp = result as CreatedResult;
            Assert.Equal(expected, resp.StatusCode);
        }
        [Fact]
        public async Task PostGenreAsync_Should_Return_Action_BadRequest() {
            // arrange
            var createDto = new GenreCreateDto();
            var Id = 0;
            _handler.Setup(a => a.CreateGenreAsync(createDto)).ReturnsAsync(Id);
            var expected = StatusCodes.Status400BadRequest;

            var controller = new GenresController(_handler.Object);
            // act
            var result = await controller.PostGenreAsync(createDto);
            // assert
            var resp = result as ObjectResult;
            Assert.Equal(expected, resp.StatusCode);
        }

        //----------------------------------------------------------------------
        // Put
        [Fact]
        public async Task PutGenreAsync_Should_Return_Action_NoContent() {
            // arrange
            var createDto = new GenreUpdateDto();
            var responseDto = new GenreResponseDto();
            _handler.Setup(a => a.UpdateGenreAsync(createDto));
            _handler.Setup(b => b.GetGenreAsync(1)).ReturnsAsync(responseDto);
            var expected = StatusCodes.Status204NoContent;

            var controller = new GenresController(_handler.Object);
            // act
            var result = await controller.PutGenreAsync(1, createDto);
            // assert
            var resp = result as NoContentResult;
            Assert.Equal(expected, resp.StatusCode);
        }

        [Fact]
        public async Task PutGenreAsync_Should_Return_Action_BadRequest() {
            // arrange
            var createDto = new GenreUpdateDto();
            _handler.Setup(a => a.UpdateGenreAsync(createDto));
            _handler.Setup(b => b.GetGenreAsync(1));
            var expected = StatusCodes.Status400BadRequest;

            var controller = new GenresController(_handler.Object);
            // act
            var result = await controller.PutGenreAsync(1, createDto);
            // assert
            var resp = result as BadRequestResult;
            Assert.Equal(expected, resp.StatusCode);
        }
    }
}
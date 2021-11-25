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
    public class CharactersControllerTest
    {
        public Mock<ICharactersHandler> _handler = new();

        //----------------------------------------------------------------------
        // delete
        [Fact]
        public async Task DeleteCharacterAsync_Should_Return_Action_NoContent() {
            // arrange
            _handler.Setup(a => a.DeleteCharacterAsync(1));
            var expected = StatusCodes.Status204NoContent;
            var controller = new CharactersController(_handler.Object);
            // act
            var result = await controller.DeleteCharacterAsync(1);
            // assert
            var resp = (StatusCodeResult)result;
            Assert.Equal(expected, resp.StatusCode);
        }
        
        //----------------------------------------------------------------------
        // Get
        [Fact]
        public async Task GetCharacterAsync_Should_Return_Action_NotFound() {
            // arrange
            var detailDto = new CharacterDetailDto();
            detailDto = null;
            _handler.Setup(a => a.GetCharacterWithFilmsAsync(1)).ReturnsAsync(detailDto);
            var expected = StatusCodes.Status404NotFound;
            var controller = new CharactersController(_handler.Object);
            // act
            var result = await controller.GetCharacterAsync(1);
            // assert
            var resp = (StatusCodeResult)result.Result;
            Assert.Equal(expected, resp.StatusCode);
        }

        [Fact]
        public async Task GetCharacterAsync_Should_Return_Action_Ok() {
            // arrange
            var detailDto = new CharacterDetailDto() { 
                CharacterId = 1, Biography="mi historia", Name="Dario", YearsOld=41, Weight=80, PathFile=""   
            };
            _handler.Setup(a => a.GetCharacterWithFilmsAsync(1)).ReturnsAsync(detailDto);
            var expected = StatusCodes.Status200OK;
            var controller = new CharactersController(_handler.Object);
            // act
            var result = await controller.GetCharacterAsync(1);
            // assert
            var resp = result.Result as ObjectResult;
            Assert.Equal(expected, resp.StatusCode);
        }

        //----------------------------------------------------------------------
        // Post
        [Fact]
        public async Task PostCharacterAsync_Should_Return_Action_Created() {
            // arrange
            var createDto = new CharacterCreateDto()
            {
                Name = "Dario", Biography = "mi historia", YearsOld = 41, Weight = 80, Films = new List<string>() { "Sherk 1", "Sherk 2"}
            };
            var Id = 1;
            _handler.Setup(a => a.CreateCharacterAsync(createDto)).ReturnsAsync(Id);
            var expected = StatusCodes.Status201Created;
            
            var controller = new CharactersController(_handler.Object);
            // act
            var result = await controller.PostCharacterAsync(createDto);
            // assert
            var resp = result as CreatedResult;
            Assert.Equal(expected, resp.StatusCode);
        }
        [Fact]
        public async Task PostCharacterAsync_Should_Return_Action_BadRequest() {
            // arrange
            var createDto = new CharacterCreateDto();
            var Id = 0;
            _handler.Setup(a => a.CreateCharacterAsync(createDto)).ReturnsAsync(Id);
            var expected = StatusCodes.Status400BadRequest;

            var controller = new CharactersController(_handler.Object);
            // act
            var result = await controller.PostCharacterAsync(createDto);
            // assert
            var resp = result as ObjectResult;
            Assert.Equal(expected, resp.StatusCode);
        }

        //----------------------------------------------------------------------
        // Put
        [Fact]
        public async Task PutCharacterAsync_Should_Return_Action_NoContent() {
            // arrange
            var createDto = new CharacterUpdateDto();
            var responseDto = new CharacterResponseDto();
            _handler.Setup(a => a.UpdateCharacterAsync(createDto));
            _handler.Setup(b => b.GetCharacterAsync(1)).ReturnsAsync(responseDto);
            var expected = StatusCodes.Status204NoContent;

            var controller = new CharactersController(_handler.Object);
            // act
            var result = await controller.PutCharacterAsync(1, createDto);
            // assert
            var resp = result as NoContentResult;
            Assert.Equal(expected, resp.StatusCode);
        }

        [Fact]
        public async Task PutCharacterAsync_Should_Return_Action_BadRequest() {
            // arrange
            var createDto = new CharacterUpdateDto();
            _handler.Setup(a => a.UpdateCharacterAsync(createDto));
            _handler.Setup(b => b.GetCharacterAsync(1));
            var expected = StatusCodes.Status400BadRequest;

            var controller = new CharactersController(_handler.Object);
            // act
            var result = await controller.PutCharacterAsync(1, createDto);
            // assert
            var resp = result as BadRequestResult;
            Assert.Equal(expected, resp.StatusCode);
        }
    }
}
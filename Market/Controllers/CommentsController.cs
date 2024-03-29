﻿using IdentityModel;
using Market.Entities.Dto;
using Market.Entities.Requests;
using Market.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Market.Controllers
{
    [Route("Comments")]
    public class CommentsController : Controller
    {
        private readonly ICommentsRepository _repository;
        private readonly ICommentsLikesRepository _likeICommentsLikesRepository;
        private LoggerLib.Interfaces.ILogger _Logger;
        public CommentsController(ICommentsRepository commentsRepository, ICommentsLikesRepository likeICommentsLikesRepository, LoggerLib.Interfaces.ILogger logger)
        {
            _repository = commentsRepository;
            _Logger = logger;
            _likeICommentsLikesRepository = likeICommentsLikesRepository;
        }

        [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync([FromBody] AddCommentRequest request)
        {
            try
            {
                await _repository.AddAsync(request, GetUserInfo());
                return Ok();
            }
            catch (Exception ex)
            {
                _Logger.Error($"Ошибка при добавлении комментария {ex.Message}");
                return StatusCode(500);
            }

        }
        [Authorize]
        [HttpPost("LikeComment")]
        public async Task<IActionResult> LikeComment([FromQuery] int id)
        {
            try
            {
                await _likeICommentsLikesRepository.LikeComment(id, GetUserInfo());
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            try
            {
                await _repository.RemoveComment(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }


        [NonAction]
        private UserInfo GetUserInfo()
        {
            return new UserInfo()
            {
                Id = User.FindFirstValue(JwtClaimTypes.ClientId),
                Username = User.FindFirstValue("username"),
            };

        }



    }
}

using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helper;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController : BaseApiController
    {
        public IUserRepository _userRepository { get; }
        public ILikesRepository _likesRepository { get; }
        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _userRepository = userRepository;
            _likesRepository = likesRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var likedUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

            if(likedUser == null) return NotFound();

            if(sourceUser.UserName == username) return BadRequest("You cannot like yourself");

            var userlike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);

            if(userlike != null) return BadRequest("You already like this user.");

            userlike = new UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = likedUser.Id
            };

            sourceUser.LikeUsers.Add(userlike);

            if( await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to like user");
        }
        [HttpGet]
        public async Task<ActionResult<PageList<LikesDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await _likesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalCount));

            return Ok(users);
        }
    }
}
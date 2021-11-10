using System;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers;
using GreenPoint.Dotnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace GreenPoint.Dotnet.WebApi.Controllers
{
    [Route("spots/{spotId}/comments/{commentId}/likes")]
    public class LikesController: ControllerBase
    {
        private readonly LikeProvider _likeProvider;
        private readonly CommentProvider _commentProvider;
        private readonly UserAuthenticationService _authenticationService;

        public LikesController(LikeProvider likeProvider, UserAuthenticationService authenticationService, CommentProvider commentProvider)
        {
            _likeProvider = likeProvider;
            _commentProvider = commentProvider;
            _authenticationService = authenticationService;
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddLike(Guid spotId, Guid commentId)
        {
            try
            {
                var user = await _authenticationService
                    .GetUserByHeaders(Request.Headers[HeaderNames.Authorization].ToArray());
            
                var likes = await _likeProvider.GetAllByUserIdAndCommentId(user.Id, commentId);
                var comment = await _commentProvider.GetById(commentId);
            
                if(likes.Count != 0) 
                {
                    return BadRequest("You can not send like to this comment");
                }
            
                var like = new Like
                {
                    CommentId = commentId,
                    UserId = user.Id
                };
                await _likeProvider.Add(like);
            
                return NoContent();
            }
            catch (ArgumentException e)
            {
                return BadRequest("Can not to add a new like");
            }
        }
    }
}
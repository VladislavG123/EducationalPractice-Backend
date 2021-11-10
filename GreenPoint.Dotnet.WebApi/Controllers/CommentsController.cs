using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;
using GreenPoint.Dotnet.Contracts.Parameters;
using GreenPoint.Dotnet.Contracts.ViewModels;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers;
using GreenPoint.Dotnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace GreenPoint.Dotnet.WebApi.Controllers
{
    [Route("/api")]
    public class CommentsController : ControllerBase
    {
        private readonly CommentProvider _commentProvider;
        private readonly AvatarProvider _avatarProvider;
        private readonly SpotProvider _spotProvider;
        private readonly UserProvider _userProvider;
        private readonly LikeProvider _likeProvider;
        private readonly UserAuthenticationService _authenticationService;
        private readonly StatusProvider _statusProvider;

        public CommentsController(CommentProvider commentProvider, UserProvider userProvider,
            LikeProvider likeProvider, AvatarProvider avatarProvider, SpotProvider spotProvider, 
            UserAuthenticationService authenticationService, StatusProvider statusProvider)
        {
            _commentProvider = commentProvider;
            _userProvider = userProvider;
            _likeProvider = likeProvider;
            _avatarProvider = avatarProvider;
            _spotProvider = spotProvider;
            _authenticationService = authenticationService;
            _statusProvider = statusProvider;
        }

        [HttpGet("spots/{spotId}/comments")]
        public async Task<IActionResult> GetAllOfSpot(Guid spotId)
        {
            var comments = await _commentProvider.GetAllBySpotId(spotId);
            var commentsViewModel = new List<CommentViewModel>();

            foreach (var comment in comments)
            {
                var user = await _userProvider.GetById(comment.UserId);
                var countLikes = await _likeProvider.GetCount(comment.Id);
                var avatar = await _avatarProvider.GetById(user.AvatarId);
                var status = await _statusProvider.GetByRating(await _userProvider.CountRating(user.Id));
                
                commentsViewModel.Add(new CommentViewModel
                {
                    Id = comment.Id,
                    AuthorsUsername = user.Username,
                    CreationDate = comment.CreationDate.ToString("g"),
                    Likes = countLikes,
                    AuthorsAvatarUrl = avatar?.Url,
                    AuthorsStatus = status.Name,
                    Text = comment.Text
                });
            }
            return Ok(commentsViewModel);
        }

        [Authorize]
        [HttpPost("spots/{spotId}/comments")]
        public async Task<IActionResult> CreateAComment(Guid spotId, [FromBody] PostCommentParameter parameter)
        {
            var geoSelect = new GeoCoordinate(parameter.Latitude, parameter.Longitude);
            var spot = await _spotProvider.GetById(spotId);
            var geoSpot = new GeoCoordinate(spot.Latitude, spot.Langitude);
            double distanceTo = geoSelect.GetDistanceTo(geoSpot); //meters

            if(distanceTo > 400)
            {
                return BadRequest("Distance is more than 400 meters");
            }

            var user = await _authenticationService
                .GetUserByHeaders(Request.Headers[HeaderNames.Authorization].ToArray());
            var comment = new Comment
            {
                SpotId = spotId,
                Text = parameter.Text,
                UserId = user.Id
            };
            await _commentProvider.Add(comment);
            return NoContent();
        }

        [Authorize]
        [HttpGet("comments")]
        public async Task<IActionResult> GetUsersComments()
        {
            var user = await _authenticationService
                .GetUserByHeaders(Request.Headers[HeaderNames.Authorization].ToArray());
            
            var comments = await _commentProvider.GetAllByUserId(user.Id);

            var commentsViewModel = new List<CommentOfUserViewModel>();

            foreach (var comment in comments)
            {
                var countLikes = await _likeProvider.GetCount(comment.Id);
                var spot = await _spotProvider.GetById(comment.SpotId);
                
                commentsViewModel.Add(new CommentOfUserViewModel
                {
                    Id = comment.Id,
                    SpotTitle = spot.Title,
                    CreationDate = comment.CreationDate.ToString("g"),
                    Likes = countLikes,
                    SpotId = comment.SpotId,
                    Text = comment.Text
                });
            }
            return Ok(commentsViewModel);
        }

        [Authorize]
        [HttpDelete("comments/{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            try
            {
                var user = await _authenticationService
                    .GetUserByHeaders(Request.Headers[HeaderNames.Authorization].ToArray());
            
                var comment = await _commentProvider.GetById(id);

                if (comment.UserId != user.Id)
                {
                    return Forbid("This comment is owns of other user");
                }
            
                await _commentProvider.Remove(comment);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return NotFound("Comment is not found");
            }
           
        }


    }
}
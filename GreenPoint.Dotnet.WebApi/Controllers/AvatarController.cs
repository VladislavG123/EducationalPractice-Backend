using System.Linq;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess.Providers;
using Microsoft.AspNetCore.Mvc;

namespace GreenPoint.Dotnet.WebApi.Controllers
{
    [Route("avatars")]
    public class AvatarController : ControllerBase
    {
        private readonly AvatarProvider _avatarProvider;

        public AvatarController(AvatarProvider avatarProvider)
        {
            _avatarProvider = avatarProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok((await _avatarProvider.GetAll()).Select(x => new
            {
                x.Id,
                x.Url
            }));
        }
    }
}
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPoint.Dotnet.DataAccess.Providers
{
    public class CommentProvider : EntityProvider<ApplicationContext, Comment, Guid>
    {
        private readonly ApplicationContext _context;

        public CommentProvider(ApplicationContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<List<Comment>> GetAllBySpotId(Guid spotId)
        {
            var comments= await this.Get(x =>
                x.SpotId.ToString().ToLower().Equals(spotId.ToString().ToLower()));
            return comments ?? throw new ArgumentException("Comments are not found");
        }
        
        public async Task<List<Comment>> GetAllByUserId(Guid userId)
        {
            var comments= await this.Get(x =>
                x.UserId.ToString().ToLower().Equals(userId.ToString().ToLower()));
            return comments ?? throw new ArgumentException("Comments are not found");
        }
    }

}

using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPoint.Dotnet.DataAccess.Providers
{
    public class LikeProvider : EntityProvider<ApplicationContext, Like, Guid>
    {
        private readonly ApplicationContext _context;

        public LikeProvider(ApplicationContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<int> GetCount(Guid commentId)
        {
            var likes = await this.Get(x =>
                 x.CommentId.ToString().ToLower().Equals(commentId.ToString().ToLower()));
            return likes.Count;
        }

        public async Task<List<Like>> GetAllByCommentId(Guid commentId)
        {
            var likes = await this.Get(x =>
                 x.CommentId.ToString().ToLower().Equals(commentId.ToString().ToLower()));
            return likes ?? throw new ArgumentException("Likes are not found");
        }

        public async Task<List<Like>> GetAllByUserIdAndCommentId(Guid userId, Guid commentId)
        {
            var likes = await this.Get(x =>
                 x.UserId.ToString().ToLower().Equals(userId.ToString().ToLower()) && x.CommentId.ToString().ToLower().Equals(commentId.ToString().ToLower()));
            return likes ?? throw new ArgumentException("Likes are not found");
        }

    }

}

using GreenPoint.Dotnet.DataAccess.Providers.Abstract;
using GreenPoint.Dotnet.DataAccess.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GreenPoint.Dotnet.DataAccess.Providers
{
    public class UserProvider : EntityProvider<ApplicationContext, User, Guid>
    {
        private readonly ApplicationContext _context;

        public UserProvider(ApplicationContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<User> GetByEmailOrPhone(string email)
        {
            var user = await this.FirstOrDefault(x =>
                x.Email.ToLower().Equals(email.ToLower()));

            return user ?? throw new ArgumentException("User is not found");
        }

        public async Task<int> CountRating(Guid userId)
        {
            return await CountRating(userId, new DateTime(1900, 1, 1));
        }
        
        public async Task<int> CountRating(Guid userId, DateTime startDate)
        {
            var likes = await
                (from like in _context.Likes
                    join comment in _context.Comments on like.CommentId equals comment.Id
                    where comment.UserId == userId
                    where like.CreationDate > startDate
                    select 1).CountAsync();

            var comments = await
                (from comment in _context.Comments
                    where comment.UserId == userId
                    where comment.CreationDate > startDate
                    select 1).CountAsync();

            return likes + 10 * comments;
        }
    }
}
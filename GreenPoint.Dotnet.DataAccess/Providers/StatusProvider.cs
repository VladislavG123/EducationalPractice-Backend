using System;
using System.Linq;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers.Abstract;
using Microsoft.EntityFrameworkCore;

namespace GreenPoint.Dotnet.DataAccess.Providers
{
    public class StatusProvider : EntityProvider<ApplicationContext, Status, Guid>
    {
        private readonly ApplicationContext _context;

        public StatusProvider(ApplicationContext context) : base(context)
        {
            this._context = context;
        }

        public override async Task Add(Status added)
        {
            var entries = await
                (from status in _context.Statuses
                    where (added.MinValue >= status.MinValue && added.MinValue < status.MaxValue) ||
                          (added.MaxValue >= status.MinValue && added.MaxValue < status.MaxValue)
                    select 1).CountAsync();
            
            if (entries != 0)
            {
                throw new ArgumentException("The new status crosses one of old ones");
            }
            
            await base.Add(added);
        }

        public async Task<Status> GetByRating(int rating)
        {
            return await FirstOrDefault(x => x.MinValue <= rating && x.MaxValue > rating);
        }
    }
}
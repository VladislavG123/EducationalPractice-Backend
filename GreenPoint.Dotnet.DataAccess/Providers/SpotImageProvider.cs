using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPoint.Dotnet.DataAccess.Providers
{
    public class SpotImageProvider : EntityProvider<ApplicationContext, SpotImage, Guid>
    {
        private readonly ApplicationContext _context;

        public SpotImageProvider(ApplicationContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<List<SpotImage>> GetBySpotId(Guid id)
        {
            var spotImages = await this.Get(x =>
                x.SpotId.ToString().ToLower().Equals(id.ToString().ToLower()));

            return spotImages ?? throw new ArgumentException("Images are not found");
        }
    }

}

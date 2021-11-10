using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPoint.Dotnet.DataAccess.Providers
{
    public class SpotProvider : EntityProvider<ApplicationContext, Spot, Guid>
    {
        private readonly ApplicationContext _context;

        public SpotProvider(ApplicationContext context) : base(context)
        {
            this._context = context;
        }

    }

}

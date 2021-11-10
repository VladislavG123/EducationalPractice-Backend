using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPoint.Dotnet.DataAccess.Providers
{
    public class AvatarProvider : EntityProvider<ApplicationContext, Avatar, Guid>
    {
        private readonly ApplicationContext _context;

        public AvatarProvider(ApplicationContext context) : base(context)
        {
            this._context = context;
        }

        
    }

}

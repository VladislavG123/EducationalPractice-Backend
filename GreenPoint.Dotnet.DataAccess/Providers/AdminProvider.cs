using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPoint.Dotnet.DataAccess.Providers
{
    public class AdminProvider : EntityProvider<ApplicationContext, Admin, Guid>
    {
        private readonly ApplicationContext _context;

        public AdminProvider(ApplicationContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<Admin> GetByLogin(string login)
        {
            return await FirstOrDefault(x => x.Login.Equals(login)) ??
                 throw new ArgumentException("No such admin in database");
        }
    }

}

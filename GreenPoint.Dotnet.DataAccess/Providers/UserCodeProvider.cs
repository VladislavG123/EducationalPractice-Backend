using System;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Providers.Abstract;

namespace GreenPoint.Dotnet.DataAccess.Providers
{
    public class UserCodeProvider: EntityProvider<ApplicationContext, UserCode, Guid>
    {
        public UserCodeProvider(ApplicationContext context) : base(context)
        {
        }
    }
}
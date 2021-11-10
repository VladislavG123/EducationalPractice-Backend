using GreenPoint.Dotnet.DataAccess.Models.Abstract;

namespace GreenPoint.Dotnet.DataAccess.Models
{
    public class Status : Entity
    {
        public string Name { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
    }
}
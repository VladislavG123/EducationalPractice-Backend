using System;

namespace GreenPoint.Dotnet.Contracts.ViewModels
{
    public class CommentOfUserViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string SpotTitle { get; set; }
        public int Likes { get; set; }
        public string CreationDate { get; set; }
        public  Guid SpotId { get; set; }
    }
}
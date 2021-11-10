using System;

namespace GreenPoint.Dotnet.Contracts.ViewModels
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string AuthorsUsername { get; set; }
        public string AuthorsAvatarUrl { get; set; }
        public string AuthorsStatus { get; set; }
        public string Text { get; set; }
        public int Likes { get; set; }
        public string CreationDate { get; set; }
    }
}
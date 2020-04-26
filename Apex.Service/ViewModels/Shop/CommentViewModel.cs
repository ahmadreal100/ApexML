using System;
using System.ComponentModel.DataAnnotations;
using Apex.Service.Translations;

namespace Apex.Service.ViewModels.Shop
{
    public class CommentViewModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long RelatedId { get; set; }
        [Display(Name = "متن پیام"), Required(ErrorMessage = "متن پیام را وارد نمایید.")]
        public string Content { get; set; }
        public bool Seen { get; set; }
        public string UserFirstName { get; set; }
        public string UserUserName { get; set; }
        public DateTime AddedDate { get; set; }
    }
    public class CommentDto
    {
        [Display(Name = nameof(Str.message)), Required(ErrorMessage = nameof(Str.rq))]
        public string Content { get; set; }
        public string UserUserName { get; set; }
        public DateTime AddedDate { get; set; }
        public long UserId { get; set; }
    }
}
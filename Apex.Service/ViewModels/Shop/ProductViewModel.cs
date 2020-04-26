using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Apex.Service.Enums;
using Apex.Service.Extensions;
using AutoMapper;

namespace Apex.Service.ViewModels.Shop
{
    public class ProductViewModel
    {
        public long Id { get; set; }

        [Display(Name = "عنوان"), Required(ErrorMessage = "عنوان را وارد نمایید.")]
        public string Title { get; set; }
        [Display(Name = "توضیحات")]
        [AllowHtml]
        public string FullDescription { get; set; }

        [Display(Name = "دسته بندی")]
        public long? CategoryId { get; set; }
        public List<ProductPictureViewModel> Pictures { get; set; } = new List<ProductPictureViewModel>();
        public List<ProductPictureViewModel> OrderedPicture => Pictures?.OrderByDescending(x => x.DisplayOrder).ToList();
        [Display(Name = "تگ")]
        public List<TagViewModel> Tags { get; set; } = new List<TagViewModel>();

        public string TagsInput { get; set; }

        public string Link => LinkCreator.Create(LinkType.Product, Id, Title);

        public DateTime AddedDate { get; set; }

        //public bool IsInGroup { get; set; }
        public void SetTags()
        {
            Tags = TagsInput?.Split(',').Select(x => new TagViewModel { Title = x }).ToList();
        }

        public void SetTagsInput()
        {
            TagsInput = string.Join(",", Tags.Select(x => x.Title));
        }

        public ProductThumbUiModel Thumb()
        {
            return Mapper.Map<ProductThumbUiModel>(this);
        }
    }
    public class ProductThumbUiModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PicLink { get; set; }
        public string Link => LinkCreator.Create(LinkType.Product, Id, Name);
        public DateTime AddedDate { get; set; }
        public long? CategoryId { get; set; }
    }
}
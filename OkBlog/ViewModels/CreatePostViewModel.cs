using Microsoft.AspNetCore.Http;
using OkBlog.Models.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OkBlog.ViewModels
{
    public class CreatePostViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; } = String.Empty;
        [Required]
        [Display(Name = "Body")]
        public string Body { get; set; } = String.Empty;
        [Display(Name = "Image")]
        public IFormFile Image { get; set; } = null;


        public List<TagViewModel> Tags { get; set; }
    }
}

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
        [Required]
        [Display(Name = "Author")]
        public string Author { get; set; } = String.Empty;

        public List<TagViewModel> Tags { get; set; }
    }
}

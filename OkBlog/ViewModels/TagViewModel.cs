using System.ComponentModel.DataAnnotations;

namespace OkBlog.ViewModels
{
	public class TagViewModel
	{
		public int Id { get; set; }
		[Required]
		[Display(Name = "Name")]
		public string Name { get; set; }
		public bool IsSelected { get; set; }
	}
}

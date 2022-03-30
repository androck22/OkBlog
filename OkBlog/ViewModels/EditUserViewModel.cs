using OkBlog.Models.Db;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OkBlog.ViewModels
{
	public class EditUserViewModel
	{
		public string FirstName { get; set; } = "name";
		public string LastName { get; set; } = "last name";
		public string Id { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
	}
}

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

		[DataType(DataType.Password)]
		[Display(Name = "Current password")]
		public string CurrentPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "New password")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm new password")]
		[Compare("NewPassword", ErrorMessage =
			"The new password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}
}

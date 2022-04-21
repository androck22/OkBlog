using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Contracts.Models.Comments
{
    public class AddCommentModel
    {
        [Required]
        public string Message { get; set; }
        [Required]
        public int PostId { get; set; }
    }
}

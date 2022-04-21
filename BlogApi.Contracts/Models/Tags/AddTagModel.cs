using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Contracts.Models.Tags
{
    public class AddTagModel
    {
        [Required]
        public string Name { get; set; }
    }
}

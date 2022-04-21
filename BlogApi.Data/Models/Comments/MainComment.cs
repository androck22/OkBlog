using System.Collections.Generic;

namespace BlogApi.Data.Models.Comments
{
    public class MainComment : Comment
    {
        public List<SubComment> SubComments { get; set; }
    }
}

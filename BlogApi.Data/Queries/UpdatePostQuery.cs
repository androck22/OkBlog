using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Data.Queries
{
    public class UpdatePostQuery
    {
        public string NewTitle { get; }
        public string NewBody { get; }

        public UpdatePostQuery(string newTitle = null, string newBody = null)
        {
            NewTitle = newTitle;
            NewBody = newBody;
        }
    }
}

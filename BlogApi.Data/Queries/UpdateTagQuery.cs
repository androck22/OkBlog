using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Data.Queries
{
    public class UpdateTagQuery
    {
        public string NewName { get; set; }

        public UpdateTagQuery(string newName = null)
        {
            NewName = newName;
        }
    }
}

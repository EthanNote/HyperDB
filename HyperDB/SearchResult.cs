using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperDB
{
    public class SearchResult
    {
        public DBNode Result { get; private set; }
        public bool Match { get; private set; }
        public SearchResult(DBNode result, bool match)
        {
            Result = result;
            Match = match;
        }
    }
}

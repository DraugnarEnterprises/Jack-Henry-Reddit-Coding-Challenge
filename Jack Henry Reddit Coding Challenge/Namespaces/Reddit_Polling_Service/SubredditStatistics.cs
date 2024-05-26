using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reddit_Polling_Service
{
    public struct SubredditStatistics
    {
        public string subredditName;
        public string subredditAuthor;
        public int? subredditAuthorPostCount;
        public string postTitle;
        public string postAuthor;
        public int? upVotes;
        public DateTime? datePolled;
    } 
}

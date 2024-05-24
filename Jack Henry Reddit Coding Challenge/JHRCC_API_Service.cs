using System;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Reddit;
using Reddit.Controllers;
using Reddit.Controllers.Structures;
using RestSharp;

namespace Jack_Henry_Reddit_Coding_Challenge
{
    internal class JHRCC_API_Service
    {
        private Subreddit _subreddit;
        private RedditClient _redditClient;

        private SubredditStatistics _subredditStatistics;
        internal SubredditStatistics subredditStatistics
        {
            get { return _subredditStatistics; }
        }

        internal JHRCC_API_Service()
        {
            Console.WriteLine("Initializing JHRCC_Reddit_Connection");
            if (_redditClient == null)
            {
                string refreshToken = AuthorizeRedditUser.AuthorizeUser(appId: ConfigurationManager.AppSettings["ClientID"], appSecret: ConfigurationManager.AppSettings["ClientSecret"]);
                _redditClient = new RedditClient(appId: ConfigurationManager.AppSettings["ClientID"], appSecret: ConfigurationManager.AppSettings["ClientSecret"], userAgent: "jackhenry/1.0.0", refreshToken: refreshToken);
            }

            Console.WriteLine($"Username: {_redditClient.Account.Me.Name}");
            Console.WriteLine($"Cake Day: {_redditClient.Account.Me.Created.ToString("D")}");
            _subreddit = _redditClient.Subreddit(name: ConfigurationManager.AppSettings["Subreddit"]);

            Query_JHRCC_API();
        }

        private void Query_JHRCC_API()
        {
            DateTime lastTimeStamp = DateTime.Now;

            List<Post> posts = _subreddit.Posts.GetTop(limit: 1);
            int requestCount = 1;

            // Begin: Get Contributor/Author with the most posts
            //    Notes: Non-moderators cannot get a list of Contributors so have to load every post in the subreddit and use Lambda to group and get a count for each Contributor/Author
            //           Requests through the API are limited to 100 results each
            List<Post> postsForAuthor = _subreddit.Posts.GetTop(limit: 100);
            List<Post> nextSet = new List<Post>(postsForAuthor);
            requestCount++;

            while (nextSet.Count > 0)
            {
                nextSet = _subreddit.Posts.GetTop(after: postsForAuthor[postsForAuthor.Count - 1].Fullname);
                if (nextSet.Count > 0) postsForAuthor.AddRange(nextSet);
                requestCount++;
                CheckRequestCountAndTime(ref requestCount, ref lastTimeStamp);
            }

            var countByAuthor = postsForAuthor.Where(post => post.Author != "[deleted]")
                                              .GroupBy(post => post.Author)
                                              .Select(postGroup => new { Author = postGroup.Key, AuthorCount = postGroup.Count() })
                                              .OrderByDescending(postGroup => (postGroup.AuthorCount)).ToList();
            // End: Get Contributor/Author with the most posts

            _subredditStatistics.subredditName = _subreddit.Name;
            _subredditStatistics.subredditAuthor = countByAuthor[0].Author;
            _subredditStatistics.subredditAuthorPostCount = countByAuthor[0].AuthorCount;
            _subredditStatistics.postTitle = posts[0].Title;
            _subredditStatistics.postAuthor = posts[0].Author;
            _subredditStatistics.upVotes = posts[0].UpVotes;
            _subredditStatistics.datePolled = System.DateTime.Now;

            Task task = Task.Run(() => { Query_JHRCC_API(); });
        }

        private void CheckRequestCountAndTime(ref int requestCount, ref DateTime lastTimeStamp)
        {
            // There is a limit of 60 requests per minute
            // We will add until 60 is reached then pause if a minute hasn't passed

            while (lastTimeStamp.AddSeconds(requestCount) > DateTime.Now)
            {
                // Do nothing until one second has passed for each request recently called
            }
            requestCount = 0;
            lastTimeStamp = DateTime.Now;
        }

        public SubredditStatistics RetrieveStats()
        {
            return _subredditStatistics;
        }
    }
}

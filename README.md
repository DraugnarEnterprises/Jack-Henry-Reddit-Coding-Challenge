Jack Henry: Reddit Coding Challenge

Overview
========
This system is a registered script app for personal use on Reddit that continually monitors Reddit for a registered user. The app is a basic console app that requires very little user interaction. It returns the highest rated post by UpVotes as well as the Contributor/Author who has made the most posts for a specified Subreddit subscribed to by the user.


Configuration Details
=====================
The App.config file contains the setting used for connecting to the Reddit API to authenticate the user of the app and to select the Subreddit to monitor. Below is a copy of the App.config settings with the Client ID and Client Secret removed.

&lt;add key="ClientID" value="&lt;Your App's ID&gt;"/&gt;<br/>
&lt;add key="ClientSecret" value="&lt;Your App's Secret&gt;"/&gt;<br/>
&lt;add key="Subreddit" value="worldnews"/&gt;<br/>
&lt;add key="OAuthRedirect" value="http://127.0.0.1:8080/Reddit.NET/oauthRedirect"/&gt;<br/>
&lt;add key="BrowserExePath" value="C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"/&gt;<br/>


Special Notes
=============

OOAuth2 Authorization
---------------------
-	The OAuth2 code (AuthorizeRedditUser) is not original code. It was only slightly modified by me to use the AppsSttings.
-	The Redirect URI for your Reddit app must be as specified in the OAuthRedirect setting. The package has a known issue that it only works with this specific URI and that the URI passed into the package must match the URI of the Reddit app. This is a minor nuisance.
-	Finally, the URI redirected too is ugly. I have no control over that. If I had time or this were going out into the real world, I would pull down the source for the library from GitHub and look into fixing the bug and making the default URI page look much nicer.

Performance
-----------
Unless the user is a Moderator, the list of Contributors cannot be polled, so the code has to get all of the posts on the Subreddit and aggregate the results of posts by user itself. The [deleted] user naturally has the most posts, so I remove it from the results before grouping.

Additionally, with a limit of 100 results per request, the list of posts has to be pulled into a list via a while loop.


Design / Code Structure Decisions
=================================

Statistics
----------
-	I created an internal struct that contains the latest statistics for the Subreddit along with the DateTime they were polled.
-	This could be used by any sort of API / web service to return the results to an external application or web page.
-	Program.cs updates with the latest when requested if the DateTime stamp has changed since the last request and waits until it does if the time stamp hasn't changed.

Request Limits from Reddit
--------------------------
Reddit limits the request count to 60 per minute. The code checks, at most, once every three requests and makes certain at least as many seconds have passed as the requests that have occured. If more than 1 request occured, the code adds an extra second to the check to be certain the 60 per minute limit cannot be exceeded. This maximizes the number of checks without exceeding Reddits limits. On my internet, this wouldn't be a problem anyhow, but it would work if this code were used in Cloud based solution to prevent Azure or some other cloud service from making too many requests.


Final Thoughts
==============
Depending on your internet speed, the inital startup may take some time. If you see your username and cake date, it is working so please be patient.

I look forward to discussing my design and my thoughts behind it further with you or someone else from Jack Henry in the future.

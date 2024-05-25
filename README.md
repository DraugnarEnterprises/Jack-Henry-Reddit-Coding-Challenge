Jack Henry: Reddit Coding Challenge

Overview
========

This system is a registered script app for personal use on Reddit that continually monitors Reddit for a registered user. The app is a basic console app that requires very little user interaction. It returns the highest rated post by UpVotes as well as the Contributor/Author who has made the most posts for a specified Subreddit subscribed to by the user.

Configuration Details
=====================

The App.config file contains the setting used for connecting to the Reddit API to authenticate the user of the app and to select the Subreddit to monitor. Below is a copy of the App.config with the Client ID and Client Secret removed.

<code>
&lt;configuration&gt;
	&lt;appSettings&gt;
 		&lt;add key="ClientID" value="Your App's AppID"/&gt;
		&lt;add key="ClientSecret" value="Your App's App Secret"/&gt;
		&lt;add key="Subreddit" value="worldnews"/&gt;
		&lt;add key="OAuthRedirect" value="http://127.0.0.1:8080/Reddit.NET/oauthRedirect"/&gt;
		&lt;add key="BrowserExePath" value="C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"/&gt;
 	&lt;/appSettings&gt;
&lt;/configuration&gt;
</code>

Special Notes
=============

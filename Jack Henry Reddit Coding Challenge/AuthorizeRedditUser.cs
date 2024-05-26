using Reddit.AuthTokenRetriever;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jack_Henry_Reddit_Coding_Challenge
{
    internal static class AuthorizeRedditUser
    {
        public static string AuthorizeUser(string appId, string appSecret, int port = 8080)
        {
            // Create a new instance of the auth token retrieval library.  --Kris
            AuthTokenRetrieverLib authTokenRetrieverLib = new AuthTokenRetrieverLib(appId, port, null, ConfigurationManager.AppSettings["OAuthRedirect"], appSecret);

            // Start the callback listener.  --Kris
            // Note - Ignore the logging exception message if you see it.  You can use Console.Clear() after this call to get rid of it if you're running a console app.
            try
            {
                authTokenRetrieverLib.AwaitCallback();
            }
            catch (Exception ex)
            {
                //Ignoring exception per noted from nuget package developer. --Robert
                Console.Clear();
            }

            // Open the browser to the Reddit authentication page.  Once the user clicks "accept", Reddit will redirect the browser to localhost:8080, where AwaitCallback will take over.  --Kris
            OpenBrowser(authTokenRetrieverLib.AuthURL(), ConfigurationManager.AppSettings["BrowserExePath"]);

            while(string.IsNullOrEmpty(authTokenRetrieverLib.RefreshToken)) { }

            // Cleanup.  --Kris
            authTokenRetrieverLib.StopListening();
            Console.Clear();


            return authTokenRetrieverLib.RefreshToken;
        }

        public static void OpenBrowser(string authUrl, string browserExePath)
        {
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(authUrl);
                Process.Start(processStartInfo);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                // This typically occurs if the runtime doesn't know where your browser is.  Use BrowserPath for when this happens.  --Kris
                ProcessStartInfo processStartInfo = new ProcessStartInfo(browserExePath)
                {
                    Arguments = authUrl
                };
                Process.Start(processStartInfo);
            }
        }
    }
}

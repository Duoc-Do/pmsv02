using Microsoft.Web.WebPages.OAuth;

namespace WebApp
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "271094843065745",
                appSecret: "a2ad8653db329289da738e327fa45e32");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "490144247742334",
            //    appSecret: "806db447d13c29a2a60c40ba1a2d9650");



            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}

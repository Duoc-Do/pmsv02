using System;
using System.Web.Security;
using WebApp.Models;


namespace WebApp.Services.Logging
{
    public static class LoggingExtensions
    {
        public static void Debug(this ILogger logger, string message, Exception exception = null, MembershipUser membershipuser = null)
        {
            FilteredLog(logger, SenLogLevel.Debug, message, exception, membershipuser);
        }
        public static void Information(this ILogger logger, string message, Exception exception = null, MembershipUser membershipuser = null)
        {
            FilteredLog(logger, SenLogLevel.Information, message, exception, membershipuser);
        }
        public static void Warning(this ILogger logger, string message, Exception exception = null, MembershipUser membershipuser = null)
        {
            FilteredLog(logger, SenLogLevel.Warning, message, exception, membershipuser);
        }
        public static void Error(this ILogger logger, string message, Exception exception = null, MembershipUser membershipuser = null)
        {
            FilteredLog(logger, SenLogLevel.Error, message, exception, membershipuser);
        }
        public static void Fatal(this ILogger logger, string message, Exception exception = null, MembershipUser membershipuser = null)
        {
            FilteredLog(logger, SenLogLevel.Fatal, message, exception, membershipuser);
        }

        private static void FilteredLog(ILogger logger, SenLogLevel level, string message, Exception exception = null, MembershipUser membershipuser = null)
        {
            //don't log thread abort exception
            if ((exception != null) && (exception is System.Threading.ThreadAbortException))
                return;

            if (logger.IsEnabled(level))
            {
                string fullMessage = exception == null ? string.Empty : exception.ToString();
                logger.InsertLog(level, message, fullMessage, membershipuser);
            }
        }
    }
}

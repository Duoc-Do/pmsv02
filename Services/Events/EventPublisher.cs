//using System;
//using System.Linq;
////using WebApp.Core.Infrastructure;
////using WebApp.Core.Plugins;
//using WebApp.Services.Logging;

//namespace WebApp.Services.Events
//{
//    /// <summary>
//    /// Evnt publisher
//    /// </summary>
//    public class EventPublisher : IEventPublisher
//    {
//        private readonly ISubscriptionService _subscriptionService;

//        /// <summary>
//        /// Ctor
//        /// </summary>
//        /// <param name="subscriptionService"></param>
//        public EventPublisher(ISubscriptionService subscriptionService)
//        {
//            _subscriptionService = subscriptionService;
//        }

//        /// <summary>
//        /// Publish to cunsumer
//        /// </summary>
//        /// <typeparam name="T">Type</typeparam>
//        /// <param name="x">Event consumer</param>
//        /// <param name="eventMessage">Event message</param>
//        protected virtual void PublishToConsumer<T>(IConsumer<T> x, T eventMessage)
//        {
//            ////Ignore not installed plugins
//            //var plugin = FindPlugin(x.GetType());
//            //if (plugin != null && !plugin.Installed)
//            //    return;

//            try
//            {
//                x.HandleEvent(eventMessage);
//            }
//            catch (Exception exc)
//            {
//                //log error
//                //var logger = EngineContext.Current.Resolve<ILogger>();
//                //we put in to nested try-catch to prevent possible cyclic (if some error occurs)

//                var _db = new Models.SenContext();
//                var httpcontextbase = new System.Web.HttpContextWrapper(System.Web.HttpContext.Current);
//                WebApp.Services.Logging.ILogger logger = new WebApp.Services.Logging.DefaultLogger(_db, httpcontextbase);
//                logger.Error(exc.Message, exc, System.Web.Security.Membership.GetUser());

//                try
//                {
//                    logger.Error(exc.Message, exc);
//                }
//                catch (Exception)
//                {
//                    //do nothing
//                }
//            }
//        }

//        ///// <summary>
//        ///// Find a plugin descriptor by some type which is located into its assembly
//        ///// </summary>
//        ///// <param name="providerType">Provider type</param>
//        ///// <returns>Plugin descriptor</returns>
//        //protected virtual PluginDescriptor FindPlugin(Type providerType)
//        //{
//        //    if (providerType == null)
//        //        throw new ArgumentNullException("providerType");

//        //    if (PluginManager.ReferencedPlugins == null)
//        //        return null;

//        //    foreach (var plugin in PluginManager.ReferencedPlugins)
//        //    {
//        //        if (plugin.ReferencedAssembly == null)
//        //            continue;

//        //        if (plugin.ReferencedAssembly.FullName == providerType.Assembly.FullName)
//        //            return plugin;
//        //    }

//        //    return null;
//        //}

//        /// <summary>
//        /// Publish event
//        /// </summary>
//        /// <typeparam name="T">Type</typeparam>
//        /// <param name="eventMessage">Event message</param>
//        public void Publish<T>(T eventMessage)
//        {
//            var subscriptions = _subscriptionService.GetSubscriptions<T>();
//            subscriptions.ToList().ForEach(x => PublishToConsumer(x, eventMessage));
//        }

//    }
//}

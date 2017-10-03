using WebApp.Services.Tasks;
using System.Net;

namespace WebApp.Services
{
    /// <summary>
    /// Represents a task for keeping the site alive
    /// </summary>
    public partial class KeepAliveTask : ITask
    {
        //private readonly StoreInformationSettings _storeInformationSettings;
        //public KeepAliveTask(StoreInformationSettings storeInformationSettings)
        //{
        //    this._storeInformationSettings = storeInformationSettings;
        //}

        public KeepAliveTask()
        {
        }

        /// <summary>
        /// Executes a task
        /// </summary>
        public void Execute()
        {
            string url = string.Format("{0}/{1}", WebApp.Services.GlobalVariant.UrlRoot, "keepalive");
            using (var wc = new WebClient())
            {
                wc.DownloadString(url);
            }
        }
    }
}

using System.Threading.Tasks;
using WebApp.Models;
using Microsoft.AspNet.SignalR;

namespace WebApp.Hubs
{
    [Authorize]
    public class AppHub : Hub
    {
        //phan nhom thong tin theo companyid

        private InMemoryAppRepository _repository;

        public AppHub()
        {
            this._repository = InMemoryAppRepository.GetInstance();
        }

        public override Task OnConnected()
        {

            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        /// <summary>
        /// Fired when a client disconnects from the system. The user associated with the client ID gets deleted from the list of currently connected users.
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            string companyId = this._repository.GetCompanyConnectionId(Context.ConnectionId);
            if (companyId != null)
            {
                this._repository.RemoveMapping(Context.ConnectionId);
                Groups.Remove(Context.ConnectionId, companyId);
            }

            return base.OnDisconnected(stopCalled);
        }

        public void Leave(string connectionid)
        {
            string companyId = this._repository.GetCompanyConnectionId(connectionid);
            if (companyId != null)
            {
                this._repository.RemoveMapping(connectionid);
                Groups.Remove(connectionid, companyId);
            }
        }

        /// <summary>
        /// Fired when a client joins the chat. Here round trip state is available and we can register the user in the list
        /// </summary>
        public string Joined(string companyid)
        {
            if (string.IsNullOrEmpty(companyid)) return Context.ConnectionId;

            string companyId = this._repository.GetCompanyConnectionId(Context.ConnectionId);
            if (companyId==null )
            {
                this._repository.AddMapping(Context.ConnectionId, companyid);
                Groups.Add(Context.ConnectionId, companyid);
            }
            return Context.ConnectionId;
        }


        #region Chat event handlers

        /// <summary>
        /// Fired when a client pushes a message to the server.
        /// </summary>
        /// <param name="message"></param>
        public void PosTableRefresh(string tableid)
        {
            Clients.Group(this._repository.GetCompanyConnectionId(Context.ConnectionId), Context.ConnectionId).onPosTableRefresh(tableid);
        }
        public void PosTakeawayRefresh(string orderid)
        {
            Clients.Group(this._repository.GetCompanyConnectionId(Context.ConnectionId), Context.ConnectionId).onPosTakeawayRefresh(orderid);
        }

        public void PosPrintKitchen(int orderid, int tableid)
        {
            Clients.Group(this._repository.GetCompanyConnectionId(Context.ConnectionId)).onPosPrintKitchen(orderid, tableid);
        }

        public void PosTableRefreshKitchen(int orderid)
        {
            Clients.Group(this._repository.GetCompanyConnectionId(Context.ConnectionId)).onPosTableRefreshKitchen(orderid);
        }

        public void PosKitchenQuantityProcess(string idupdate)
        {
            Clients.Group(this._repository.GetCompanyConnectionId(Context.ConnectionId)).onPosKitchenQuantityProcess(idupdate);
        }
        #endregion

    }
}
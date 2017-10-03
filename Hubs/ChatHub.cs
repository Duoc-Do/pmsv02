using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApp.Models;
using Microsoft.AspNet.SignalR;

namespace WebApp.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private InMemoryRepository _repository;

        public ChatHub()
        {
            _repository = InMemoryRepository.GetInstance();
        }

        #region IDisconnect and IConnected event handlers implementation
        public override Task OnConnected()
        {
            Joined();
            return base.OnConnected();
        }

        /// <summary>
        /// Fired when a client disconnects from the system. The user associated with the client ID gets deleted from the list of currently connected users.
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            string userId = _repository.GetUserByConnectionId(Context.ConnectionId);
            if (userId != null)
            {
                _repository.RemoveMapping(Context.ConnectionId);
                Groups.Remove(Context.ConnectionId, userId);
                var userconnect = _repository.GetConnectionByUserId(userId);
                if (userconnect.Count == 0)
                {
                    ChatUser user = _repository.Users.Where(u => u.UserId == userId).FirstOrDefault();
                    _repository.Remove(user);
                    return Clients.All.leaves(user.SenUserId, user.UserId, user.UserName, DateTime.Now);
                }
            }

            return base.OnDisconnected(stopCalled);
        }

        #endregion

        #region Chat event handlers

        /// <summary>
        /// Fired when a client pushes a message to the server.
        /// </summary>
        /// <param name="message"></param>
        public void Send(ChatMessage message)
        {
            if (!string.IsNullOrEmpty(message.Content))
            {
                // Sanitize input
                message.Content = HttpUtility.HtmlEncode(message.Content);
                // Process URLs: Extract any URL and process rich content (e.g. Youtube links)
                //HashSet<string> extractedURLs;
                //message.Content = TextParser.TransformAndExtractUrls(message.Content, out extractedURLs);
                message.Timestamp = DateTime.Now;
                Clients.All.onMessageReceived(message);
                //Clients.All.thongbao(message.Content);

            }
        }

        /// <summary>
        /// Fired when a client pushes a message to the server.
        /// </summary>
        /// <param name="message"></param>
        public void SendPM(string userid, ChatMessage message)
        {
            if (!string.IsNullOrEmpty(message.Content))
            {
                // Sanitize input
                message.Timestamp = DateTime.Now;
                AddChatMessage(message.Content, message.Timestamp, GetRemoteIpAddress(Context.Request), message.UserId, userid);
                message.Content = HttpUtility.HtmlEncode(message.Content);
                // Process URLs: Extract any URL and process rich content (e.g. Youtube links)
                //HashSet<string> extractedURLs;
                //message.Content = TextParser.TransformAndExtractUrls(message.Content, out extractedURLs);

                Clients.User(userid).onMessageReceived(message);
                Clients.Caller.onMessageReceived(message);

            }
        }

        /// <summary>
        /// Fired when a client pushes a message to the server.
        /// </summary>
        /// <param name="message"></param>
        public void SendToGroup(string groupname, ChatMessage message)
        {
            if (!string.IsNullOrEmpty(message.Content))
            {
                // Sanitize input
                message.Content = HttpUtility.HtmlEncode(message.Content);
                // Process URLs: Extract any URL and process rich content (e.g. Youtube links)
                //HashSet<string> extractedURLs;
                //message.Content = TextParser.TransformAndExtractUrls(message.Content, out extractedURLs);
                message.Timestamp = DateTime.Now;

                Clients.Caller.onMessageReceived(message);
                Clients.Group(groupname).onMessageReceived(message);

            }
        }

        /// <summary>
        /// Fired when a client joins the chat. Here round trip state is available and we can register the user in the list
        /// </summary>
        public void Joined()
        {
            string userName = Context.User.Identity.Name;
            var db = new SenContext();
            var senuser = db.SenUsers.Where(m => m.UserProfile.UserName == userName).SingleOrDefault();

            string userId = _repository.GetUserByConnectionId(Context.ConnectionId);


            if (userId == null)
            {
                var _isSupport = Context.User.IsInRole("SupportOnline");
                ChatUser user = new ChatUser()
                {
                    SenUserId = senuser.SenUserId,
                    UserId = userName,
                    UserName = senuser.FullName,
                    Avatar = WebApp.Services.Media.PictureService.GetPictureUrl(senuser.Avatar, 120, 120),
                    LastActive = DateTime.Now,
                    IsSupport = _isSupport
                };
                _repository.Add(user);
                _repository.AddMapping(Context.ConnectionId, user.UserId);
                if (_isSupport)
                {
                    //nếu là support online thì thông báo hết cho client
                    Clients.All.joins(user.SenUserId, user.UserId, user.UserName, user.LastActive, user.Avatar);
                }
                else // còn nếu không phải support thì thông báo cho support thôi
                {
                    var _users = _repository.Users.Where(m => m.IsSupport).Select(m => m.UserId).ToList();
                    Clients.Users(_users).joins(user.SenUserId, user.UserId, user.UserName, user.LastActive, user.Avatar);
                }
                Groups.Add(Context.ConnectionId, user.UserId);
            }
            else
            {
                Clients.All.thongbao("bạn đã đăng nhập rồi");
            }
        }

        /// <summary>
        /// Invoked when a client connects. Retrieves the list of all currently connected users
        /// </summary>
        /// <returns></returns>
        public ICollection<ChatUser> GetConnectedUsers()
        {
            var _isRole = Context.User.IsInRole("Admin") || Context.User.IsInRole("SupportOnline");
            if (_isRole)
            {
                //Nếu là quản trị hoặc hỗ trợ trực tuyến thì sẽ lấy hết danh sách online
                return _repository.Users.ToList<ChatUser>();
            }
            else
            {
                //Nếu user thường thì lấy danh sách user Support.
                return _repository.Users.Where(m => m.IsSupport).ToList<ChatUser>();
            }
        }

        public string GetNoReadCount(string userid)
        {
            string _kq = "";
            var db = new SenContext();
            _kq = db.SenChatMessages.Where(m => m.UserId == userid && m.RefUserId == Context.User.Identity.Name && m.IsRead == false).Count().ToString("n0");
            return _kq;
        }

        public ICollection<SenChatMessage> RecentMessage(string userid, int maxid = 0)
        {
            var db = new SenContext();

            var query = db.SenChatMessages.Where(m => (m.RefUserId == userid && m.UserId == Context.User.Identity.Name) || (m.UserId == userid && m.RefUserId == Context.User.Identity.Name));
            if (maxid > 0)
            {
                query = query.Where(m => m.Id < maxid);
            }

            return query.OrderByDescending(m => m.Id).Take(10).ToList();
        }
        #endregion

        #region Update database
        public string GetRemoteIpAddress(IRequest request)
        {
            object ipAddress;
            if (request.Environment.TryGetValue("server.RemoteIpAddress", out ipAddress))
            {
                return ipAddress as string;
            }
            return null;
        }

        private void AddChatMessage(string Content, DateTime Timestamp, string IpAddress, string UserId, string RefUserId)
        {
            var db = new SenContext();
            var add = new WebApp.Models.SenChatMessage()
            {
                Content = Content,
                Timestamp = Timestamp,
                UserId = UserId,
                RefUserId = RefUserId,
                IpAddress = IpAddress,
                IsRead = false
            };
            db.SenChatMessages.Add(add);
            db.SaveChanges();
        }

        public int SetRead(string userid)
        {
            var db = new SenContext();
            var q = db.SenChatMessages.Where(m => m.UserId == userid && m.RefUserId == Context.User.Identity.Name && m.IsRead == false).Count();
            if (q > 0)
            {
                string query = string.Format("Update SenChatMessage set IsRead=1 Where IsRead=0 and UserId='{0}' and RefUserId='{1}'", userid, Context.User.Identity.Name);
                return db.Database.ExecuteSqlCommand(query);
            }
            return 0;
        }

        #endregion
    }
}
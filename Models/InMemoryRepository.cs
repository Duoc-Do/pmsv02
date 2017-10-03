using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.Models
{
    public class InMemoryRepository
    {
        private static ICollection<ChatUser> _connectedUsers;
        private static Dictionary<string, string> _mappings;
        private static InMemoryRepository _instance = null;
        private static readonly int max_random = 3;
        
        public static InMemoryRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new InMemoryRepository();                
            }
            return _instance;
        }

        #region Private methods

        private InMemoryRepository()
        {
            _connectedUsers = new List<ChatUser>();
            _mappings = new Dictionary<string, string>();
        }

        #endregion

        #region Repository methods

        public IQueryable<ChatUser> Users { get { return _connectedUsers.AsQueryable(); } }

        public void Add(ChatUser user)
        {
            var _user=this.Users.Where(m => m.UserId == user.UserId).SingleOrDefault();
            if (_user==null)
            {
                _connectedUsers.Add(user);
            }
        }

        public void Remove(ChatUser user)
        {
            _connectedUsers.Remove(user);
        }

        public string GetRandomizedUsername(string username)
        {
            string tempUsername = username;
            int newRandom = max_random, oldRandom = 0;
            int loops = 0;
            Random random = new Random();
            do
            {
                if (loops > newRandom)
                {
                    oldRandom = newRandom;
                    newRandom *= 2;
                }
                username = tempUsername + "_" + random.Next(oldRandom, newRandom).ToString();
                loops++;
            } while (GetInstance().Users.Where(u => u.UserName.Equals(username)).ToList().Count > 0);

            return username;
        }

        public void AddMapping(string connectionId, string userId)
        {
            if (!string.IsNullOrEmpty(connectionId) && !string.IsNullOrEmpty(userId))
            {
                _mappings.Add(connectionId, userId);
            }
        }

        public void RemoveMapping(string connectionId)
        {
            if (!string.IsNullOrEmpty(connectionId))
            {
                _mappings.Remove(connectionId);
            }
        }

        public string GetUserByConnectionId(string connectionId)
        {
            string userId = null;
            _mappings.TryGetValue(connectionId, out userId);            
            return userId;
        }

        public List<KeyValuePair<string,string>> GetConnectionByUserId(string userId)
        {
            return _mappings.Where(m => m.Value == userId).ToList();
        }
        #endregion


    }


    public class InMemoryAppRepository
    {
        private static Dictionary<string, string> _mappings;
        private static InMemoryAppRepository _instance = null;
        public static InMemoryAppRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new InMemoryAppRepository();
            }
            return _instance;
        }

        #region Private methods

        private InMemoryAppRepository()
        {
            _mappings = new Dictionary<string, string>();
        }

        #endregion

        #region Repository methods

        public void AddMapping(string connectionId, string companyId)
        {
            if (!string.IsNullOrEmpty(connectionId) && !string.IsNullOrEmpty(companyId))
            {
                _mappings.Add(connectionId, companyId);
            }
        }

        public void RemoveMapping(string connectionId)
        {
            if (!string.IsNullOrEmpty(connectionId))
            {
                _mappings.Remove(connectionId);
            }
        }

        public string GetCompanyConnectionId(string connectionId)
        {
            string companyId = null;
            _mappings.TryGetValue(connectionId, out companyId);
            return companyId;
        }

        public List<KeyValuePair<string, string>> GetConnectionByCompanyId(string companyId)
        {
            return _mappings.Where(m => m.Value == companyId).ToList();
        }
        #endregion


    }

}
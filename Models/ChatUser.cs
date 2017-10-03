using System;

namespace WebApp.Models
{
    public class ChatUser
    {
        public ChatUser()
        {
            IsSupport = false;
        }

        public string UserId { get; set; }        
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public DateTime LastActive { get; set; }
        public int SenUserId { get; set; }
        public bool IsSupport { get; set; }
    }
}
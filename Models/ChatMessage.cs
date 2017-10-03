using System;

namespace WebApp.Models
{
    public class ChatMessage
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public string Avatar { get; set; }
    }
}
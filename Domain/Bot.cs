using System;

namespace Domain
{
    public class Bot
    {
        public Guid Id { get; set; }
        public string TMIToken { get; set; }
        public string TwitchClientId { get; set; }
        public string Nick { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}

using System;

namespace Domain
{
    public class Settings
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TargetChannel { get; set; }
        public string Prefix { get; set; }
        public int ReminderTimer { get; set; }
        public string FolderId { get; set; }
    }
}

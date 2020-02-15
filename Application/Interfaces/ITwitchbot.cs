using Domain;

namespace Application.Interfaces
{
    public interface ITwitchbot
    {
        public bool IsRunning { get; set; }
        public Bot Bot { get; set; }
        public Settings Settings { get; set; }
        public bool StartBot();
        public void StopBot();
    }
}
using System.Diagnostics;

namespace SleepTimer.Commands
{
    public class Shutdown : ProcessShutdownCommand
    {
        // string shutdownWithoutWaitingArgs = "/s /t 0";
        public Shutdown()
            : base("/s /t 0")
        {
        }
    }
}
using System.Diagnostics;

namespace SleepTimer.Commands
{
    public class Restart : ProcessShutdownCommand
    {
        // string restartArgs = "/r";
        public Restart()
            : base("/r")
        {
        }
    }
}
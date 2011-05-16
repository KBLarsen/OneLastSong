using System.Diagnostics;

namespace SleepTimer.Commands
{
    public class Restart : ICommand
    {
        private readonly string shutdownCommand = "shutdown";
        private readonly string restartFlag = "/r";

        public void Execute()
        {
            Process.Start(shutdownCommand, restartFlag);            
        }
    }
}
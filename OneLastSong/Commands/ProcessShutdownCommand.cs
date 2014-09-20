using System.Diagnostics;

namespace SleepTimer.Commands
{
    public abstract class ProcessShutdownCommand : ICommand
    {
        private readonly string arguments;

        public ProcessShutdownCommand(string arguments)
        {
            this.arguments = arguments;
        }

        public void Execute()
        {
            Process.Start("shutdown", arguments);            
        }
    }
}
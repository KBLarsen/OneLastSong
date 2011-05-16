using System.Windows.Forms;

namespace SleepTimer.Commands
{
    public class Standby : ICommand
    {
        private readonly bool force = true;
        private readonly bool disableWakeEvent = true;

        public void Execute()
        {
            Application.SetSuspendState(PowerState.Suspend, force, disableWakeEvent);
        }
    }
}
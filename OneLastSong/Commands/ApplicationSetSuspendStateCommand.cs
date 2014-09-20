using System.Windows.Forms;

namespace SleepTimer.Commands
{
    public abstract class ApplicationSetSuspendStateCommand : ICommand
    {
        private readonly PowerState powerstate;
        private readonly bool force;
        private readonly bool disableWakeEvent;

        public ApplicationSetSuspendStateCommand(PowerState powerstate, bool force, bool disableWakeEvent)
        {
            this.powerstate = powerstate;
            this.force = force;
            this.disableWakeEvent = disableWakeEvent;
        }

        public void Execute()
        {
            Application.SetSuspendState(powerstate, force, disableWakeEvent);
        }
    }
}
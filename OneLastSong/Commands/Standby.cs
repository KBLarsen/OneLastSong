using System.Windows.Forms;

namespace SleepTimer.Commands
{
    public class Standby : ApplicationSetSuspendStateCommand
    {
        public Standby()
            : base(PowerState.Suspend, true, true)
        {
        }
    }

}
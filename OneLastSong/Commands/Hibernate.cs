using System.Windows.Forms;

namespace SleepTimer.Commands
{
    public class Hibernate : ApplicationSetSuspendStateCommand
    {
        public Hibernate()
            : base(PowerState.Hibernate, true, false)
        {
        }
    }
}
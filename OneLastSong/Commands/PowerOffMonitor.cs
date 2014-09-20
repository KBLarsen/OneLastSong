using System;
using System.Runtime.InteropServices;

namespace SleepTimer.Commands
{
    public class PowerOffMonitor : ICommand
    {
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public enum MonitorPowerState
        {
            StandBy = 1,
            Off = 2
        }

        private readonly IntPtr hWnd;

        public PowerOffMonitor(IntPtr hWnd)
        {
            this.hWnd = hWnd;
        }

        public void Execute() 
        { 
            const uint SystemCommand = 0x0112;

            IntPtr MonitorPower = (IntPtr)0xF170;

            SendMessage(hWnd, SystemCommand, MonitorPower, (IntPtr)MonitorPowerState.Off);
        }
    }
}
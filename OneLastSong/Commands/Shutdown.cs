﻿using System.Diagnostics;

namespace SleepTimer.Commands
{
    public class Shutdown : ICommand
    {
        private readonly string shutdownCommand = "shutdown";
        private readonly string shutdownWithoutWaitingArgs = "/s /t 0";

        public void Execute()
        {
            Process.Start(shutdownCommand, shutdownWithoutWaitingArgs);
        }
    }
}
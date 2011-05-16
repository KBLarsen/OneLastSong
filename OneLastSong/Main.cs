﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using SleepTimer.Commands;
using SleepTimer.Properties;

namespace SleepTimer
{
    public partial class Main : Form
    {
        private static readonly Dictionary<int, ICommand> CommandMap = new Dictionary<int, ICommand>
        {
            { 0, new Hibernate() },
            { 1, new Shutdown() },
            { 2, new Standby() },
            { 3, new Restart() },
        };

        private Countdown countdown;

        public Main()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            
            Time.Text = SleepTimer.Properties.Settings.Default.DefaultCountDownTime.TotalMinutes.ToString();
            Time.Focus();
            Options.SelectedIndex = SleepTimer.Properties.Settings.Default.DefaultAction;
            DisableMonitor.Checked = SleepTimer.Properties.Settings.Default.DefaultMonitorTurnOff;
        }

        private void OnGoClick(object sender, EventArgs e)
        {
            if (Time.Text.IsEmpty() || !Time.Text.IsPositiveNumber())
            {
                MessageBox.Show("No deal, enter a number of minutes please.", "Try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if(countdown != null)
            {
                countdown.Stop();
                countdown.Tick -= SetTimeRemaining;
                countdown.Expired -= ExecuteCommand;
            }

            countdown = Countdown.FromMinutes(double.Parse(Time.Text));
            countdown.Tick += SetTimeRemaining;
            countdown.Expired += ExecuteCommand;
            countdown.Start();

            FiveMoreMinutes.Enabled = true;

            SetTimeRemaining();
        }

        private void ExecuteCommand()
        {
            if (DisableMonitor.Checked)
            {
                new PowerOffMonitor(Handle).Execute();
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            
            var command = CommandMap[Options.SelectedIndex];
            command.Execute();
        }

        private void SetTimeRemaining()
        {
            string HoursRemaining = "";
            int RemainingTotalWholeHours = (int)Math.Truncate(countdown.TimeRemaining.TotalHours);
            if (RemainingTotalWholeHours > 0)
            {
                HoursRemaining = string.Format("{0}:", RemainingTotalWholeHours);
            }
            TimeRemaining.Text = HoursRemaining + string.Format("{0:00}:{1:00}", countdown.TimeRemaining.Minutes, countdown.TimeRemaining.Seconds);
        }

        private void OnFiveMoreMinutesClick(object sender, EventArgs e)
        {
            countdown.IncreaseDuration(TimeSpan.FromMinutes(5));
            SetTimeRemaining();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using SleepTimer.Commands;
using SleepTimer.Properties;
using System.Drawing;

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

        /*
        private enum DurationParseFormatError
        {
            Ok,
            UnkownError,
            HoursParseError,
            HoursRangeError,
            MinutesParseError,
            MinutesRangeError,
            SecondsParseError,
            SecondsRangeError
        }

        private bool DurationParse(string Timetext, out int format, out double minutes, out DurationParseFormatError formatError)
        */
        private bool DurationParse(string Timetext, out int format, out double minutes, out string formatErrorMsg)
        {
            // formatError = DurationParseFormatError.UnkownError;
            formatErrorMsg = "";
            string [] NumberStrings = Timetext.Split(new char[] {':'}, 3);
            int NoOfTimeParts  = NumberStrings.Length;
            if (NoOfTimeParts == 1)
            {   // "123"  minutes
                if (double.TryParse(NumberStrings[0], out minutes))
                {
                    if (minutes >= 0)
                    {
                        format = 1;
                        // formatError = DurationParseFormatError.Ok;
                        return true;
                    }
                    // formatError = DurationParseFormatError.MinutesRangeError;
                    formatErrorMsg = "Minutes Range Error";
                }
                else
                {
                    // formatError = DurationParseFormatError.MinutesParseError;
                    formatErrorMsg = "Minutes Parse Error";
                }
            } 
            else if (NoOfTimeParts == 2)
            {   // "12:34" hours:minutes
                double hours, mins;
                if (double.TryParse(NumberStrings[0], out hours))
                {
                    if (hours >= 0)
                    {
                        if (double.TryParse(NumberStrings[1], out mins))
                        {
                            if ((mins >= 0) && (mins < 60))
                            {
                                minutes = hours * 60 + mins;
                                format = 2;
                                return true;
                            }
                            // formatError = DurationParseFormatError.MinutesRangeError;
                            formatErrorMsg = "Minutes Range Error";
                        }
                        else
                        {
                            // formatError = DurationParseFormatError.MinutesParseError;
                            formatErrorMsg = "Minutes Parse Error";
                        }
                    }
                    else
                    {
                        // formatError = DurationParseFormatError.HoursRangeError;
                        formatErrorMsg = "Hours Range Error";
                    }
                }
                else
                {
                    // formatError = DurationParseFormatError.HoursParseError;
                    formatErrorMsg = "Hours Parse Error";
                }
            } 
            else if (NoOfTimeParts == 3)
            {   // "12:34:56" hours:minutes:seconds
                double hours, mins, secs;
                if (double.TryParse(NumberStrings[0], out hours))
                {
                    if (hours >= 0)
                    {
                        if (double.TryParse(NumberStrings[1], out mins))
                        {
                            if ((mins >= 0) && (mins < 60))
                            {
                                mins = hours * 60 + mins;
                                if (double.TryParse(NumberStrings[2], out secs))
                                {
                                    if ((secs >= 0) && (secs < 60))
                                    {
                                        minutes = mins + secs / 60;
                                        format = 3;
                                        return true;
                                    }
                                    else
                                    {
                                        // formatError = DurationParseFormatError.SecondsRangeError;
                                        formatErrorMsg = "Seconds Range Error";
                                    }
                                }
                                else
                                {
                                    // formatError = DurationParseFormatError.SecondsParseError;
                                    formatErrorMsg = "Seconds Parse Error";
                                }
                            }
                            else
                            {
                                // formatError = DurationParseFormatError.MinutesRangeError;
                                formatErrorMsg = "Minutes Range Error";
                            }
                        }
                        else
                        {
                            // formatError = DurationParseFormatError.MinutesParseError;
                            formatErrorMsg = "Minutes Parse Error";
                        }
                    }
                    else
                    {
                        // formatError = DurationParseFormatError.HoursRangeError;
                        formatErrorMsg = "Hours Range Error";
                    }
                }
                else
                {
                    // formatError = DurationParseFormatError.HoursParseError;
                    formatErrorMsg = "Hours Parse Error";
                }
            }

            format = 0;
            minutes = 0;
            return false;
        }
        
        private void Time_TextChanged(object sender, EventArgs e)
        {
            int format = 1;
            double minutes = 0;
            string formatError = label2.Text;
            if (!Time.Text.IsEmpty() )
            {
                DurationParse(Time.Text, out format, out minutes, out formatError);
            }

            if (format == 1)
            {
                label2.Text = "minutes";
            }
            else
            if (format == 2)
            {
                label2.Text = "hours:minutes";
            } 
            else
            if (format == 3)
            {
                label2.Text = "hours:minutes:seconds";
            }
            else
            {
                label2.Text = formatError;
            }

            Time.ForeColor = (format == 0) ? Color.Red : SystemColors.WindowText;
            label2.ForeColor = (format == 0) ? Color.Red : SystemColors.WindowText;
        }

        private void OnGoClick(object sender, EventArgs e)
        {
            if (countdown != null)
            {
                countdown.Stop();
                countdown.Tick -= SetTimeRemaining;
                countdown.Expired -= ExecuteCommand;
                FiveMoreMinutes.Enabled = false;
            }

            int format;
            double minutes;
            if (Time.Text.IsEmpty())
            {
                MessageBox.Show("Enter a number of minutes please.", "Try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string formatEror; 
            if (!DurationParse(Time.Text, out format, out minutes, out formatEror ))
            {
                MessageBox.Show(formatEror + ".\nEnter a time as Min or H:MM or H:MM:SS please.", "Try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            countdown = Countdown.FromMinutes(minutes);
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

        private void DoItNowButton_Click(object sender, EventArgs e)
        {
            ExecuteCommand();
        }
    }
}
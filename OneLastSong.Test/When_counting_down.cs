using System;
using System.Threading;
using NUnit.Framework;

namespace OneLastSong.Test
{
    [TestFixture]
    [Category("Countdown")]
    public class When_counting_down
    {
        private readonly Countdown countdown = Countdown.FromMinutes(1);
        private readonly ManualResetEvent expired = new ManualResetEvent(false);
        private int ticks;

        public When_counting_down()
        {
            countdown.Tick += () => ticks++;
            countdown.Expired += () => expired.Set();
            countdown.Start();
            
            expired.WaitOne(TimeSpan.FromSeconds(5));
        }

        [Test]
        public void It_should_tick_five_times()
        {
            Assert.That(ticks, Is.EqualTo(5));
        }

        [Test]
        public void It_should_show_fifty_five_seconds_remaining()
        {
            Assert.That(countdown.TimeRemaining, Is.EqualTo(TimeSpan.FromSeconds(55)));
        }
    }
}
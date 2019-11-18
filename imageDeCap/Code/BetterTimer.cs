using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageDeCap
{
    // replacement for windows's default timer because I need more accuracy
    public class BetterTimer : IDisposable
    {
        public static List<BetterTimer> Timers = new List<BetterTimer>();

        public object Tag = null;
        public double FramesPerSecond = 20.0;

        public delegate void TickDelegate();
        public TickDelegate Tick;

        public BetterTimer()
        {
            Timers.Add(this);
        }
        public void Dispose()
        {
            Timers.Remove(this);
        }

        DateTime LastPokeTime;
        double AccumulatedTime = 0;
        bool First = true;
        public void TickPrivate()
        {
            if(First)
            {
                First = false;
                LastPokeTime = DateTime.Now;
            }
            TimeSpan TimeSinceLastCapture = DateTime.Now - LastPokeTime;
            LastPokeTime = DateTime.Now;
            double DeltaTime = TimeSinceLastCapture.Ticks / 10000.0 / 1000.0;
            double FrameTime = 1.0 / FramesPerSecond;

            if (AccumulatedTime > 0.5)//Something went wrong and we stalled for half a second, reset to 0.
            {
                AccumulatedTime = 0;
            }
            AccumulatedTime += DeltaTime;
            if(AccumulatedTime > FrameTime)
            {
                double rest = AccumulatedTime - FrameTime;
                AccumulatedTime = rest;

                Tick.Invoke();
            }
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.Misc
{
    public class DStopper
    {
        private Stopwatch stopper = new Stopwatch();

        private TimeSpan offset = new TimeSpan();

        public TimeSpan Elapsed
        {
            get
            {
                return stopper.Elapsed + offset;
            }
            set
            {
                offset = value;
            }
        }

        public bool IsRunning
        {
            get
            {
                return stopper.IsRunning;
            }
        }

        public void Start()
        {
            stopper.Start();
        }

        public void Stop()
        {
            stopper.Stop();
        }

        public void Reset()
        {
            stopper.Reset();
        }

        public void ResetOffset()
        {
            offset = new TimeSpan();
        }
    }
}

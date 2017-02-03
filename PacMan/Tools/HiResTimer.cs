﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace PacMan
{
    class HiResTimer
    {
        Stopwatch stopwatch;

        public HiResTimer()
        {
            stopwatch = new Stopwatch();
            stopwatch.Reset();
        }

        public long ElapsedMilliseconds
        {
            get { return stopwatch.ElapsedMilliseconds; }
        }

        public void Start()
        {
            if (!stopwatch.IsRunning)
            {
                stopwatch.Reset();
                stopwatch.Start();
            }
        }

        public void Stop()
        {
            stopwatch.Stop();
        }
    }
}
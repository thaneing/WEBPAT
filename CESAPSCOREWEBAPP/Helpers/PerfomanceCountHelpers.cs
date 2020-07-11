using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Helpers
{
    public class PerfomanceCountHelpers
    {
        public static long SessionCount(string CategoryName, string CounterName, string InstanceName)
        {
            PerformanceCounter performanceCounter = new System.Diagnostics.PerformanceCounter();
            performanceCounter.CategoryName = "Web Service";
            performanceCounter.CounterName = "Current Connections";
            performanceCounter.InstanceName = "LicenKey";

            return performanceCounter.RawValue;
        }
    }
}

using System;
using System.Diagnostics;
using System.Threading;

namespace Destiny.IO
{
    public static class Tracer
    {
        #region traceError
        public static void TraceErrorMessage(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log.Error("Error message: {0}", message);
            Log.Error("Method/Property name: {0}", memberName);
            Log.Error("Source file path: {0}", sourceFilePath);
            Log.Error("Source file line number: {0}", sourceLineNumber);
        }

        public static void TraceErrorMessage(string message, 
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0, params object[] args)
        {
            Log.Error("Error message: {0}", message);
            Log.Error("Method/Property name: {0}", memberName);
            Log.Error("Source file path: {0}", sourceFilePath);
            Log.Error("Source file line number: {0}", sourceLineNumber);
            Log.Error("Additional debug info: {0}", args);
        }

        public static void TraceErrorMessage(Exception e, string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log.Error("Error message: {0}", message);
            Log.Error("Method/Property name: {0}", memberName);
            Log.Error("Source file path: {0}", sourceFilePath);
            Log.Error("Source file line number: {0}", sourceLineNumber);
            Log.Error(e);
        }
        #endregion

        #region traceWarn
        public static void TraceWarnMessage(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log.Warn("Error message: {0}", message);
            Log.Warn("Method/Property name: {0}", memberName);
            Log.Warn("Source file path: {0}", sourceFilePath);
            Log.Warn("Source file line number: {0}", sourceLineNumber);
        }

        public static void TraceWarnMessage(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0, params object[] args)
        {
            Log.Warn("Error message: {0}", message);
            Log.Warn("Method/Property name: {0}", memberName);
            Log.Warn("Source file path: {0}", sourceFilePath);
            Log.Warn("Source file line number: {0}", sourceLineNumber);
            Log.Warn("Additional debug info: {0}", args);
        }

        public static void TraceWarnMessage(Exception e, string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log.Warn("Error message: {0}", message);
            Log.Warn("Method/Property name: {0}", memberName);
            Log.Warn("Source file path: {0}", sourceFilePath);
            Log.Warn("Source file line number: {0}", sourceLineNumber);
            Log.Warn("Exception information: {0}", e);
        }
        #endregion

        #region traceInfo
        public static void TraceInfoMessage(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log.Inform("Error message: {0}", message);
            Log.Inform("Method/Property name: {0}", memberName);
            Log.Inform("Source file path: {0}", sourceFilePath);
            Log.Inform("Source file line number: {0}", sourceLineNumber);
        }

        public static void TraceInfoMessage(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0, params object[] args)
        {
            Log.Inform("Error message: {0}", message);
            Log.Inform("Method/Property name: {0}", memberName);
            Log.Inform("Source file path: {0}", sourceFilePath);
            Log.Inform("Source file line number: {0}", sourceLineNumber);
            Log.Inform("Additional debug info: {0}", args);
        }
        #endregion traceInfo


        public static double GetExecutionTime(string description, int iterations, Action func)
        {
            //Run at highest priority to minimize fluctuations caused by other processes/threads
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            // warm up 
            func();

            var watch = new Stopwatch();

            // clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            watch.Start();
            for (int i = 0; i < iterations; i++)
            {
                func();
            }
            watch.Stop();

            Log.Inform(description);
            Log.Inform(" Time Elapsed: {0} ms", watch.Elapsed.TotalMilliseconds);

            return watch.Elapsed.TotalMilliseconds;
        }
    }
}
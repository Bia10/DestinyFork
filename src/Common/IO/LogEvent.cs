using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Destiny.IO
{
    public static class LogEventQue
    {
        // A prioritized que of resolved communication events
        public static List<LogEvent> NormalEventsLog { get; set; } = new List<LogEvent>();

        // A prioritized que of unresolved communication events
        public static List<LogEvent> AnomalousEventsLog { get; set; } = new List<LogEvent>();

        public class LogEvent
        {
            /// <summary>
            /// Current time when LogEvent was registered
            /// </summary>
            [DefaultValue("")]
            public string EventTime { get; set; }

            /// <summary>
            /// Name of LogEvent registered
            /// </summary>
            [DefaultValue("")]
            public string EventName { get; set; }

            /// <summary>
            /// Byte array of actual data associated with current LogEvent
            /// </summary>
            [DefaultValue(0)]
            public byte[] EventData { get; set; }

            /// <summary>
            /// Priority of LogEvent registered, zero is highest priority
            /// </summary>
            [DefaultValue(0)]
            public int EventPriority { get; set; }

            /// <summary>
            /// Unused so far
            /// </summary>
            [DefaultValue(null)]
            public Exception EventException { get; set; }

            public static implicit operator List<object>(LogEvent v)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Sort all LogEvents by priority in particular logQue
        /// </summary>
        public static void SortLogEventsByPriority(List<LogEvent> logQueToSort)
        {
            logQueToSort.Sort((le1, le2) => le1.EventPriority.CompareTo(le2.EventPriority));
        }

        /// <summary>
        /// Check for duplicate of LogEvent in logQue
        /// </summary>
        public static bool IsDuplicate(List<LogEvent> logQueToCheck, LogEvent eventToCheck)
        {
            string firstName = eventToCheck.EventName;

            return logQueToCheck.Any(logEvent => logEvent.EventName == firstName);
        }
    }
}
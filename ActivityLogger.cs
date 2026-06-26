using System;
using System.Collections.Generic;
using System.Linq;

namespace demo
{// start of namespace

    // -----------------------------------------------------------------------
    // LOG ENTRY CLASS
    // Represents a single logged action with a timestamp and a short
    // description, e.g. "Task added: 'Enable two-factor authentication'"
    // -----------------------------------------------------------------------
    public class LogEntry
    {// start of class

        public DateTime Timestamp { get; set; }
        public string Description { get; set; }

        public LogEntry(string description)
        {
            Timestamp = DateTime.Now;
            Description = description;
        }

        // Used directly by the Activity Log GUI ListView, since binding a
        // TextBlock to "{Binding}" falls back to calling ToString()
        public override string ToString()
        {
            return "[" + Timestamp.ToString("dd MMM HH:mm") + "] " + Description;
        }

    }// end of class


    // -----------------------------------------------------------------------
    // ACTIVITY LOGGER CLASS
    // Stores a running list of every significant action the chatbot takes
    // (Part 3 / Task 4) - tasks added, reminders set, quiz attempts, and
    // commands recognised through the NLP keyword detection - so the user
    // can review what the bot has done for them.
    // -----------------------------------------------------------------------
    public class ActivityLogger
    {// start of class

        // Add a list/dictionary in code to store completed actions, as required by the brief
        private List<LogEntry> log = new List<LogEntry>();


        // -----------------------------------------------------------------------
        // LOG METHOD
        // Adds a new timestamped entry describing the action that just happened
        // -----------------------------------------------------------------------
        public void Log(string description)
        {
            log.Add(new LogEntry(description));

        }// end of Log


        // -----------------------------------------------------------------------
        // GET RECENT METHOD
        // Returns the most recent "count" log entries, newest first, so the
        // default activity log view stays short and relevant (5-10 actions)
        // -----------------------------------------------------------------------
        public List<LogEntry> GetRecent(int count)
        {
            return log
                .OrderByDescending(entry => entry.Timestamp)
                .Take(count)
                .ToList();

        }// end of GetRecent


        // -----------------------------------------------------------------------
        // GET ALL METHOD
        // Returns the complete history of logged actions, newest first -
        // used by the optional "Show Full History" button
        // -----------------------------------------------------------------------
        public List<LogEntry> GetAll()
        {
            return log
                .OrderByDescending(entry => entry.Timestamp)
                .ToList();

        }// end of GetAll


        // Total number of actions logged this session
        public int TotalCount
        {
            get { return log.Count; }
        }

    }// end of class
}// end of namespace
using System;

namespace demo
{// start of namespace

    // -----------------------------------------------------------------------
    // TASK ITEM CLASS
    // Simple data model representing one cybersecurity task.
    // Each task is loaded from / saved to the MySQL "tasks" table by
    // TaskAssistantDB. The DisplayReminder and StatusText properties are
    // used directly by the Tasks GUI list (see MainWindow.xaml tasks_grid).
    // -----------------------------------------------------------------------
    public class TaskItem
    {// start of class

        // Primary key in the tasks table
        public int Id { get; set; }

        // The username this task belongs to (so each user only sees their own tasks)
        public string Username { get; set; }

        // Short title for the task, e.g. "Enable two-factor authentication"
        public string Title { get; set; }

        // Longer optional description of the task
        public string Description { get; set; }

        // Optional reminder date - null if the user did not set a reminder
        public DateTime? ReminderDate { get; set; }

        // Whether the user has marked this task as completed
        public bool IsCompleted { get; set; }

        // When the task was first created
        public DateTime CreatedAt { get; set; }


        // -----------------------------------------------------------------------
        // DISPLAY REMINDER PROPERTY
        // Friendly string shown in the Tasks GUI list, e.g. "Reminder: 26 Jun 2026"
        // or "No reminder set" if the user did not specify one
        // -----------------------------------------------------------------------
        public string DisplayReminder
        {
            get
            {
                if (ReminderDate.HasValue)
                    return "Reminder: " + ReminderDate.Value.ToString("dd MMM yyyy");

                return "No reminder set";
            }
        }


        // -----------------------------------------------------------------------
        // STATUS TEXT PROPERTY
        // Friendly completed/pending label shown next to each task
        // -----------------------------------------------------------------------
        public string StatusText
        {
            get
            {
                return IsCompleted ? "Completed" : "Pending";
            }
        }

    }// end of class
}// end of namespace

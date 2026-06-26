using System;
using System.Text.RegularExpressions;

namespace demo
{// start of namespace

    // -----------------------------------------------------------------------
    // CHAT INTENT ENUM
    // The set of commands the NLP simulation is able to recognise from the
    // user's free-typed message, regardless of how it is worded
    // -----------------------------------------------------------------------
    public enum ChatIntent
    {
        None,
        AddTask,
        ShowTasks,
        CompleteTask,
        DeleteTask,
        StartQuiz,
        ShowActivityLog
    }


    // -----------------------------------------------------------------------
    // NLP PROCESSOR CLASS
    // Simulates basic Natural Language Processing (Part 3 / Task 3) using
    // keyword detection and simple string manipulation (string.Contains(),
    // string.StartsWith() and regular expressions) rather than a full NLP
    // library, as suggested in the brief. This lets the chatbot recognise
    // the same request phrased in different ways, e.g.
    // "add a task" vs "remind me to" vs "add task to" vs "set a reminder".
    // -----------------------------------------------------------------------
    public class NlpProcessor
    {// start of class

        // Phrases that signal the user wants to add a new cybersecurity task
        private static readonly string[] taskKeywords =
        {
            "add task", "add a task", "new task", "create task", "create a task",
            "set a task", "add to do", "add todo", "add a to do"
        };

        // Phrases that signal the user wants to set a reminder
        // ("remind me to..." is treated as creating a brand new task with a
        // reminder attached, matching the brief's example interaction)
        private static readonly string[] reminderKeywords =
        {
            "remind me", "reminder", "set a reminder", "add a reminder"
        };

        private static readonly string[] showTasksKeywords =
        {
            "show task", "show my task", "view task", "list task", "see my task", "my tasks", "what tasks"
        };

        private static readonly string[] completeKeywords =
        {
            "mark complete", "mark as complete", "complete task", "finish task", "done with task", "task complete"
        };

        private static readonly string[] deleteKeywords =
        {
            "delete task", "remove task", "cancel task"
        };

        private static readonly string[] quizKeywords =
        {
            "start quiz", "take quiz", "play quiz", "begin quiz", "quiz me",
            "test me", "start the game", "play game", "play the game", "mini game", "mini-game"
        };

        private static readonly string[] activityLogKeywords =
        {
            "activity log", "show activity", "show log", "what have you done",
            "recent actions", "show history", "show my activity"
        };

        // Phrases that mean "no" when replying to "Would you like a reminder?"
        private static readonly string[] noWords = { "no", "nope", "not now", "skip", "nah" };

        // Phrases that mean "yes" when replying to "Would you like a reminder?"
        private static readonly string[] yesWords = { "yes", "yeah", "yep", "sure", "ok", "okay", "please" };

        // Common command phrases that should be stripped off the front of a
        // message before what remains is treated as the task title
        private static readonly string[] phrasesToStrip =
        {
            "add a task to", "add task to", "add a task -", "add task -",
            "add a task", "add task", "new task to", "new task -", "new task",
            "create a task to", "create task to", "create a task", "create task",
            "set a task to", "set a task", "remind me to", "remind me",
            "set a reminder to", "set a reminder", "add a reminder to", "add a reminder"
        };

        // Timeframe words stripped off the end of a task title so they are
        // not duplicated inside the title itself, e.g. "update my password tomorrow"
        private static readonly string[] trailingTimePhrases = { "tomorrow", "today", "next week" };


        // -----------------------------------------------------------------------
        // DETECT INTENT METHOD
        // Scans the cleaned, already-lowercased-safe user input for known
        // keyword phrases and returns the matching intent. More specific
        // intents (quiz, activity log, complete/delete) are checked before
        // the more general "add task"/"reminder" intents so a message like
        // "show activity log" is never mistaken for "add task".
        // -----------------------------------------------------------------------
        public ChatIntent DetectIntent(string input)
        {
            string text = input.ToLower();

            if (ContainsAny(text, activityLogKeywords)) return ChatIntent.ShowActivityLog;
            if (ContainsAny(text, quizKeywords)) return ChatIntent.StartQuiz;
            if (ContainsAny(text, completeKeywords)) return ChatIntent.CompleteTask;
            if (ContainsAny(text, deleteKeywords)) return ChatIntent.DeleteTask;
            if (ContainsAny(text, showTasksKeywords)) return ChatIntent.ShowTasks;

            // "remind me to X" implies creating a brand new task with a
            // reminder attached, e.g. "Remind me to update my password tomorrow."
            if (ContainsAny(text, reminderKeywords)) return ChatIntent.AddTask;

            if (ContainsAny(text, taskKeywords)) return ChatIntent.AddTask;

            // Catch loosely-worded variations using individual keyword detection,
            // e.g. "Add a task to enable 2FA" still contains "task" and "add"
            // even if it doesn't exactly match one of the fixed phrases above
            if (text.Contains("task") && (text.Contains("add") || text.Contains("create") || text.Contains("new") || text.Contains("set")))
                return ChatIntent.AddTask;

            return ChatIntent.None;

        }// end of DetectIntent


        // -----------------------------------------------------------------------
        // CONTAINS ANY METHOD
        // Simple helper using string.Contains() to check a list of keywords -
        // the basic string manipulation technique suggested in the brief
        // -----------------------------------------------------------------------
        private bool ContainsAny(string text, string[] keywords)
        {
            foreach (string keyword in keywords)
            {
                if (text.Contains(keyword))
                    return true;
            }

            return false;

        }// end of ContainsAny


        // -----------------------------------------------------------------------
        // EXTRACT TASK TITLE METHOD
        // Strips the recognised command phrase off the front of the user's
        // message to leave just the task description behind, e.g.
        // "Add a task to enable two-factor authentication" -> "Enable two-factor authentication"
        // "Remind me to update my password tomorrow" -> "Update my password"
        // -----------------------------------------------------------------------
        public string ExtractTaskTitle(string cleanedInput)
        {
            string text = cleanedInput.Trim();
            string lower = text.ToLower();

            foreach (string phrase in phrasesToStrip)
            {
                if (lower.StartsWith(phrase))
                {
                    text = text.Substring(phrase.Length).Trim();
                    break;
                }
            }

            // Remove a leading dash or colon left over from phrasing like
            // "Add task - Review privacy settings"
            text = text.TrimStart('-', ':', ' ').Trim();

            // Strip a trailing timeframe so it isn't duplicated inside the task title
            text = StripTrailingTimeframe(text);

            if (string.IsNullOrWhiteSpace(text))
                return "New cybersecurity task";

            // Capitalise the first letter for a cleaner display in the GUI/chat
            text = char.ToUpper(text[0]) + text.Substring(1);

            return text.Trim();

        }// end of ExtractTaskTitle


        // -----------------------------------------------------------------------
        // STRIP TRAILING TIMEFRAME METHOD
        // Removes common date/time phrases from the end of a task description
        // -----------------------------------------------------------------------
        private string StripTrailingTimeframe(string text)
        {
            string lower = text.ToLower();

            foreach (string phrase in trailingTimePhrases)
            {
                if (lower.EndsWith(phrase))
                {
                    text = text.Substring(0, text.Length - phrase.Length).Trim();
                    break;
                }
            }

            // Strip "in X day(s)" / "in X week(s)" from the end, e.g.
            // "review privacy settings in 7 days" -> "review privacy settings"
            text = Regex.Replace(text, @"\s+in\s+\d+\s+(day|days|week|weeks)\s*$", "", RegexOptions.IgnoreCase).Trim();

            return text;

        }// end of StripTrailingTimeframe


        // -----------------------------------------------------------------------
        // TRY PARSE REMINDER DATE METHOD
        // Looks for a simple date/timeframe phrase in the user's message and
        // converts it into an actual DateTime for storage in the DB.
        // Supports: "today", "tomorrow", "next week", "in N day(s)",
        // "in N week(s)", and a plain number typed alone (e.g. a reply of "3").
        // -----------------------------------------------------------------------
        public bool TryParseReminderDate(string input, out DateTime reminderDate)
        {
            string text = input.ToLower().Trim();
            reminderDate = DateTime.Now;

            if (text.Contains("tomorrow"))
            {
                reminderDate = DateTime.Now.AddDays(1);
                return true;
            }

            if (text.Contains("next week"))
            {
                reminderDate = DateTime.Now.AddDays(7);
                return true;
            }

            if (text.Contains("today"))
            {
                reminderDate = DateTime.Now;
                return true;
            }

            // Matches phrases like "in 3 days", "in 5 day", "3 days", "remind me in 7 days"
            Match dayMatch = Regex.Match(text, @"(\d+)\s*day");
            if (dayMatch.Success)
            {
                int days = int.Parse(dayMatch.Groups[1].Value);
                reminderDate = DateTime.Now.AddDays(days);
                return true;
            }

            // Matches phrases like "in 2 weeks"
            Match weekMatch = Regex.Match(text, @"(\d+)\s*week");
            if (weekMatch.Success)
            {
                int weeks = int.Parse(weekMatch.Groups[1].Value);
                reminderDate = DateTime.Now.AddDays(weeks * 7);
                return true;
            }

            // If the reply is just a plain number (e.g. the user simply typed "3"
            // in answer to "In how many days?"), assume it means "in N days"
            if (Regex.IsMatch(text, @"^\d+$"))
            {
                int days = int.Parse(text);
                reminderDate = DateTime.Now.AddDays(days);
                return true;
            }

            return false;

        }// end of TryParseReminderDate


        // -----------------------------------------------------------------------
        // IS AFFIRMATIVE METHOD
        // Detects "yes" style replies so the bot knows the user wants a
        // reminder when asked "Would you like a reminder?"
        // -----------------------------------------------------------------------
        public bool IsAffirmative(string input)
        {
            string text = input.ToLower();
            return ContainsAny(text, yesWords);

        }// end of IsAffirmative


        // -----------------------------------------------------------------------
        // IS NEGATIVE METHOD
        // Detects "no" style replies
        // -----------------------------------------------------------------------
        public bool IsNegative(string input)
        {
            string text = input.ToLower();
            return ContainsAny(text, noWords);

        }// end of IsNegative

    }// end of class
}// end of namespace
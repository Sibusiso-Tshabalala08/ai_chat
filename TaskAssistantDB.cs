using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace demo
{// start of namespace

    // -----------------------------------------------------------------------
    // TASK ASSISTANT DB CLASS
    // Handles all MySQL database access for the Task Assistant feature
    // (Part 3 / Task 1). Responsible for creating the database and table on
    // first run, and for adding, retrieving, updating and deleting tasks.
    //
    // IMPORTANT: Update the four fields below (DbServer, DbPort, DbUser,
    // DbPassword) to match your own local MySQL installation before running
    // the project. The database name (DbName) does not need to be created
    // manually - this class creates it automatically the first time the
    // app runs, as long as the MySQL user above has permission to do so.
    // -----------------------------------------------------------------------
    public class TaskAssistantDB
    {// start of class

        // ----- MySQL connection settings - EDIT THESE FOR YOUR MACHINE -----
        private static readonly string DbServer = "localhost";
        private static readonly string DbPort = "3306";
        private static readonly string DbName = "cyberbot_db";
        private static readonly string DbUser = "root";
        private static readonly string DbPassword = "T5h4b4lal4"; // set your MySQL password here
        // ---------------------------------------------------------------------

        // Connection string used to connect to the server without selecting
        // a database yet - used only to create the database if it is missing
        private string ServerOnlyConnectionString()
        {
            return "Server=" + DbServer + ";Port=" + DbPort + ";Uid=" + DbUser + ";Pwd=" + DbPassword + ";SslMode=none;Connection Timeout=5;";
        }

        // Connection string used for all normal task operations once the
        // cyberbot_db database is known to exist
        private string FullConnectionString()
        {
            return "Server=" + DbServer + ";Port=" + DbPort + ";Database=" + DbName + ";Uid=" + DbUser + ";Pwd=" + DbPassword + ";SslMode=none;Connection Timeout=5;";
        }

        // True once the database and table have been confirmed to exist
        public bool IsConnected { get; private set; }

        // Stores the last connection error so the GUI can display a helpful message
        public string LastError { get; private set; }


        // -----------------------------------------------------------------------
        // CONSTRUCTOR
        // Automatically ensures the database and table exist as soon as the
        // chatbot starts, so the rest of the app never has to worry about it
        // -----------------------------------------------------------------------
        public TaskAssistantDB()
        {
            EnsureDatabaseAndTable();
        }


        // -----------------------------------------------------------------------
        // ENSURE DATABASE AND TABLE METHOD
        // Creates the cyberbot_db database (if missing) and the tasks table
        // (if missing). If the MySQL server cannot be reached at all, this
        // sets IsConnected to false and stores the error instead of crashing
        // the application.
        // -----------------------------------------------------------------------
        private void EnsureDatabaseAndTable()
        {
            try
            {
                // Step 1: connect to the server only and create the database if needed
                using (MySqlConnection conn = new MySqlConnection(ServerOnlyConnectionString()))
                {
                    conn.Open();

                    string createDb = "CREATE DATABASE IF NOT EXISTS " + DbName + ";";
                    using (MySqlCommand cmd = new MySqlCommand(createDb, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // Step 2: connect to the actual database and create the tasks table if needed
                using (MySqlConnection conn = new MySqlConnection(FullConnectionString()))
                {
                    conn.Open();

                    string createTable =
                        "CREATE TABLE IF NOT EXISTS tasks (" +
                        "Id INT AUTO_INCREMENT PRIMARY KEY," +
                        "Username VARCHAR(50) NOT NULL," +
                        "Title VARCHAR(255) NOT NULL," +
                        "Description TEXT," +
                        "ReminderDate DATETIME NULL," +
                        "IsCompleted BOOLEAN NOT NULL DEFAULT 0," +
                        "CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP" +
                        ");";

                    using (MySqlCommand cmd = new MySqlCommand(createTable, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                IsConnected = true;
                LastError = string.Empty;
            }
            catch (Exception ex)
            {
                // Something went wrong (server offline, wrong credentials, etc.)
                // Record the error so the GUI can show it instead of crashing
                IsConnected = false;
                LastError = ex.Message;
            }

        }// end of EnsureDatabaseAndTable


        // -----------------------------------------------------------------------
        // ADD TASK METHOD
        // Inserts a new cybersecurity task for the given user and returns the
        // new task's auto-generated Id (used so a reminder can be attached
        // to it immediately afterwards if the user requests one)
        // -----------------------------------------------------------------------
        public int AddTask(string username, string title, string description, DateTime? reminderDate)
        {
            string insert =
                "INSERT INTO tasks (Username, Title, Description, ReminderDate, IsCompleted) " +
                "VALUES (@username, @title, @description, @reminder, 0);";

            using (MySqlConnection conn = new MySqlConnection(FullConnectionString()))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(insert, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@description", description ?? string.Empty);

                    if (reminderDate.HasValue)
                        cmd.Parameters.AddWithValue("@reminder", reminderDate.Value);
                    else
                        cmd.Parameters.AddWithValue("@reminder", DBNull.Value);

                    cmd.ExecuteNonQuery();
                }

                // LAST_INSERT_ID() is scoped to the current connection/session,
                // so this correctly returns the Id of the row we just inserted above
                using (MySqlCommand idCmd = new MySqlCommand("SELECT LAST_INSERT_ID();", conn))
                {
                    object result = idCmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }

        }// end of AddTask


        // -----------------------------------------------------------------------
        // SET REMINDER METHOD
        // Updates the ReminderDate column for an existing task - used when
        // the user replies "yes" to "Would you like to set a reminder?"
        // -----------------------------------------------------------------------
        public void SetReminder(int taskId, DateTime reminderDate)
        {
            string update = "UPDATE tasks SET ReminderDate = @reminder WHERE Id = @id;";

            using (MySqlConnection conn = new MySqlConnection(FullConnectionString()))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(update, conn))
                {
                    cmd.Parameters.AddWithValue("@reminder", reminderDate);
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.ExecuteNonQuery();
                }
            }

        }// end of SetReminder


        // -----------------------------------------------------------------------
        // GET TASKS METHOD
        // Retrieves every task belonging to the given username, newest first.
        // Used to populate the Tasks GUI list and the chat-based "show my tasks"
        // command.
        // -----------------------------------------------------------------------
        public List<TaskItem> GetTasks(string username)
        {
            List<TaskItem> tasks = new List<TaskItem>();

            string select =
                "SELECT Id, Username, Title, Description, ReminderDate, IsCompleted, CreatedAt " +
                "FROM tasks WHERE Username = @username ORDER BY CreatedAt DESC;";

            using (MySqlConnection conn = new MySqlConnection(FullConnectionString()))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(select, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Resolve column ordinals once up front - safer than
                        // relying on name-based reader overloads
                        int idCol = reader.GetOrdinal("Id");
                        int userCol = reader.GetOrdinal("Username");
                        int titleCol = reader.GetOrdinal("Title");
                        int descCol = reader.GetOrdinal("Description");
                        int reminderCol = reader.GetOrdinal("ReminderDate");
                        int completeCol = reader.GetOrdinal("IsCompleted");
                        int createdCol = reader.GetOrdinal("CreatedAt");

                        while (reader.Read())
                        {
                            TaskItem task = new TaskItem();
                            task.Id = reader.GetInt32(idCol);
                            task.Username = reader.GetString(userCol);
                            task.Title = reader.GetString(titleCol);
                            task.Description = reader.IsDBNull(descCol) ? string.Empty : reader.GetString(descCol);
                            task.ReminderDate = reader.IsDBNull(reminderCol) ? (DateTime?)null : reader.GetDateTime(reminderCol);
                            task.IsCompleted = reader.GetBoolean(completeCol);
                            task.CreatedAt = reader.GetDateTime(createdCol);

                            tasks.Add(task);
                        }
                    }
                }
            }

            return tasks;

        }// end of GetTasks


        // -----------------------------------------------------------------------
        // GET MOST RECENT TASK METHOD
        // Returns the task most recently added by this user - used by the
        // chat-based NLP flow to know which task to attach a reminder to
        // -----------------------------------------------------------------------
        public TaskItem GetMostRecentTask(string username)
        {
            List<TaskItem> tasks = GetTasks(username);
            return tasks.Count > 0 ? tasks[0] : null;

        }// end of GetMostRecentTask


        // -----------------------------------------------------------------------
        // MARK COMPLETE METHOD
        // Marks the given task as completed in the database
        // -----------------------------------------------------------------------
        public void MarkComplete(int taskId)
        {
            string update = "UPDATE tasks SET IsCompleted = 1 WHERE Id = @id;";

            using (MySqlConnection conn = new MySqlConnection(FullConnectionString()))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(update, conn))
                {
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.ExecuteNonQuery();
                }
            }

        }// end of MarkComplete


        // -----------------------------------------------------------------------
        // DELETE TASK METHOD
        // Permanently removes a task from the database
        // -----------------------------------------------------------------------
        public void DeleteTask(int taskId)
        {
            string delete = "DELETE FROM tasks WHERE Id = @id;";

            using (MySqlConnection conn = new MySqlConnection(FullConnectionString()))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(delete, conn))
                {
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.ExecuteNonQuery();
                }
            }

        }// end of DeleteTask

    }// end of class
}// end of namespace
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace demo
{// start of namespace

    public class user_name
    {// start of class


        // -----------------------------------------------------------------------
        // SUBMIT NAME METHOD
        // Called from MainWindow when the user submits their username
        // Checks that the name is not empty before proceeding
        // Checks that the name is not longer than 20 characters
        // Shows a detailed welcome message for new users
        // -----------------------------------------------------------------------
        public string submit_name(TextBox user_name, ListView chats)
        {// start of submit_name

            // Name of the file used to store all registered usernames
            string filename = "user_names.txt";

            // If the file does not exist yet, create it automatically
            if (!File.Exists(filename))
            {
                // Create the file with a placeholder line so ReadAllLines never fails
                File.AppendAllText(filename, "auto_create\n");
            }

            // Get the username text and trim any accidental spaces
            string name = user_name.Text.ToString().Trim();

            // Reject empty username and show an error in the chat
            if (string.IsNullOrWhiteSpace(name))
            {
                error_method("ChatBot", "Please enter a username to continue.", chats);
                return string.Empty;
            }

            // Reject usernames that are too long
            if (name.Length > 20)
            {
                error_method("ChatBot", "Username must be 20 characters or less.", chats);
                return string.Empty;
            }

            // Check if this name already exists in the saved usernames file
            bool found = check_name(name);

            if (!found)
            {// start of if — new user

                // Save the new username to the file for future recognition
                File.AppendAllText(filename, name + "\n");

                // Greet the new user with a detailed welcome and list of topics
                error_method("ChatBot",
                    "Hey " + name + ", welcome to AI Cybersecurity! " +
                    "I am your personal cybersecurity assistant. I can help you with: " +
                    "password safety, phishing scams, malware and viruses, firewalls, VPNs, " +
                    "data breaches, encryption, two-factor authentication, and much more. " +
                    "Feel free to ask me anything about staying safe online!", chats);

            }// end of if
            else
            {// start of else — returning user

                // Welcome the returning user with a shorter familiar greeting
                error_method("ChatBot",
                    "Hey " + name + ", welcome back! How can I help keep you safe online today?", chats);

            }// end of else

            // Return the validated username back to MainWindow to store in the username variable
            return name;

        }// end of submit_name


        // -----------------------------------------------------------------------
        // CHECK NAME METHOD
        // Reads the usernames file and checks if the given name already exists
        // Returns true if the name is found, false if it is a new user
        // -----------------------------------------------------------------------
        private Boolean check_name(string name)
        {// start of check_name

            // Name of the file to read usernames from
            string filename = "user_names.txt";

            // Assume not found until proven otherwise
            bool found_name = false;

            // Read all lines from the file into a string array
            string[] names = File.ReadAllLines(filename);

            // Loop through every saved name and compare with the input
            foreach (string name_found in names)
            {// start of loop

                // Case-insensitive comparison so "John" and "john" are treated the same
                if (name_found.ToLower() == name.ToLower())
                {// start of if
                    found_name = true;
                }// end of if

            }// end of loop

            // Return whether the name was found or not
            return found_name;

        }// end of check_name


        // -----------------------------------------------------------------------
        // ERROR METHOD (DISPLAY MESSAGE)
        // Displays a styled chat message bubble in the ListView
        // Updated to use dark navy and cyan theme to match the app aesthetic
        // -----------------------------------------------------------------------
        private void error_method(string name, string message, ListView chats)
        {// start of error_method

            // Create the outer border container for this message bubble
            Border messageBorder = new Border
            {
                Margin = new Thickness(0, 2, 0, 2),
                Padding = new Thickness(5, 3, 5, 3),
                CornerRadius = new CornerRadius(5)
            };

            // Apply dark theme colors — bot gets cyan, user gets deep blue
            if (name.ToLower().Contains("chatbot") || name.ToLower().Contains("chat"))
            {
                // Bot message styling — dark navy with cyan border
                messageBorder.Background = new SolidColorBrush(Color.FromRgb(10, 14, 39));
                messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 191, 255));
            }
            else
            {
                // User message styling — slightly lighter navy with blue border
                messageBorder.Background = new SolidColorBrush(Color.FromRgb(15, 22, 58));
                messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(30, 100, 220));
            }

            messageBorder.BorderThickness = new Thickness(1);

            // Create the text block that will hold the sender name and message
            TextBlock messageText = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(2),
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 13
            };

            // Cyan for the bot name label, soft blue for the user name label
            Brush nameColor = (name.ToLower().Contains("chatbot") || name.ToLower().Contains("chat"))
                ? new SolidColorBrush(Color.FromRgb(0, 191, 255))
                : new SolidColorBrush(Color.FromRgb(100, 160, 255));

            // Off-white blue tint used for the body of each message
            Brush messageColor = new SolidColorBrush(Color.FromRgb(200, 220, 255));

            // Add the bold sender name label (e.g. "ChatBot: ")
            messageText.Inlines.Add(new Run
            {
                Text = name + ": ",
                Foreground = nameColor,
                FontWeight = FontWeights.Bold
            });

            // Add the message content text
            messageText.Inlines.Add(new Run
            {
                Text = message,
                Foreground = messageColor
            });

            // Place the text block inside the border bubble
            messageBorder.Child = messageText;

            // Add the completed message bubble to the chat ListView
            chats.Items.Add(messageBorder);

        }// end of error_method


    }// end of class
}// end of namespace

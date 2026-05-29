using demo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace demo
{// start of namespace

    public partial class MainWindow : Window
    {// start of class

        // ArrayList to store all possible chatbot answers loaded from respond.cs
        ArrayList reply = new ArrayList();

        // ArrayList to store words that should be ignored during keyword matching
        ArrayList ignore = new ArrayList();

        // Instance of user_name class used to handle username submission and checking
        user_name check_name = new user_name();

        // Stores the current user's username after it has been submitted
        string username = string.Empty;

        // Stores the previous question — reserved for future use
        string pre_question = string.Empty;

        // Counter used by auto_show_interest to track every 3 messages sent
        int counting = 0;

        // Flag to prevent auto_show_interest from calling ai_check recursively
        bool isShowingInterest = false;

        // Tracks the last cybersecurity topic the bot responded to
        // Used by conversation flow to know what to elaborate on
        string lastTopic = string.Empty;

        // List of sentiment words that trigger an automatic tip
        // When detected, bot responds with empathy AND follows up with a relevant tip
        private List<string> sentimentWords = new List<string>
        {
            "worried", "frustrated", "confused", "angry", "sad", "scared", "nervous", "curious"
        };

        // Maps each sentiment to a related cybersecurity topic
        // So when user says "I'm worried", bot automatically gives a tip about scams
        private Dictionary<string, string> sentimentToTopic = new Dictionary<string, string>
        {
            { "worried",    "phishing"       },
            { "frustrated", "password"       },
            { "confused",   "cybersecurity"  },
            { "angry",      "fraud"          },
            { "sad",        "privacy"        },
            { "scared",     "malware"        },
            { "nervous",    "vpn"            },
            { "curious",    "cybersecurity"  }
        };


        // Constructor — runs when the application first launches

        public MainWindow()
        {
            // Initialize all XAML components
            InitializeComponent();

            // Load all answers and ignored words into the reply and ignore ArrayLists
            new respond(reply, ignore) { };

            // Create an instance of the voice greeting class
            voice_greeting greet = new voice_greeting();

            // Play the welcome voice greeting audio
            greet.greet();

        }// end of constructor


        // -----------------------------------------------------------------------
        // PROCEED EVENT HANDLER
        // Triggered when the user clicks the Proceed button on the home page
        // Hides the home grid and shows the username entry grid
        // -----------------------------------------------------------------------
        private void proceed(object sender, RoutedEventArgs e)
        {
            // Hide the home landing page
            home_grid.Visibility = Visibility.Hidden;

            // Show the username input page
            username_grid.Visibility = Visibility.Visible;

        }// end of proceed


        // -----------------------------------------------------------------------
        // SUBMIT NAME EVENT HANDLER
        // Triggered when the user clicks Submit Username
        // Validates that the input is not empty before proceeding
        // Shows the error TextBlock if input is empty
        // -----------------------------------------------------------------------
        private void submit_name(object sender, RoutedEventArgs e)
        {
            // Check if the username input box is empty or only whitespace
            if (string.IsNullOrWhiteSpace(usernames_input.Text))
            {
                // Make the error message visible to inform the user
                error_text.Visibility = Visibility.Visible;

                // Stop here — do not proceed until a name is entered
                return;
            }

            // Hide the error message if it was previously shown
            error_text.Visibility = Visibility.Hidden;

            // Pass the TextBox and chat ListView to user_name class to handle login/register
            username = check_name.submit_name(usernames_input, chats);

            // Hide the username page
            username_grid.Visibility = Visibility.Hidden;

            // Show the main chat page
            chat_grid.Visibility = Visibility.Visible;

        }// end of submit_name


        // -----------------------------------------------------------------------
        // ENTER KEY HANDLER
        // Triggered when a key is pressed while the question TextBox is focused
        // If the key pressed is Enter, it calls the send method
        // -----------------------------------------------------------------------
        private void question_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the key pressed was the Enter key
            if (e.Key == Key.Enter)
            {
                // Trigger the send method as if the Send button was clicked
                send(sender, e);
            }

        }// end of question_KeyDown


        // -----------------------------------------------------------------------
        // PLACEHOLDER VISIBILITY HANDLER
        // Triggered every time the text in the question TextBox changes
        // Shows the placeholder hint when the box is empty, hides it when typing
        // -----------------------------------------------------------------------
        private void question_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Make sure the placeholder element exists before trying to access it
            if (placeholder_hint != null)
            {
                // Show placeholder if TextBox is empty, hide it if user is typing
                placeholder_hint.Visibility = string.IsNullOrEmpty(question.Text)
                    ? Visibility.Visible
                    : Visibility.Hidden;
            }

        }// end of question_TextChanged


        // -----------------------------------------------------------------------
        // SEND EVENT HANDLER (now async for typing indicator)
        // Triggered when the user clicks Send or presses Enter
        // Validates input, displays user message, shows typing indicator,
        // then passes the question to the AI logic
        // -----------------------------------------------------------------------
        private async void send(object sender, RoutedEventArgs e)
        {
            // Get the text from the question input box and remove leading/trailing spaces
            string rawQuestion = question.Text.ToString().Trim();

            // Check if the user typed anything meaningful
            if (string.IsNullOrWhiteSpace(rawQuestion))
            {
                // Inform the user they need to type something
                error_method("ChatBot", "Please enter a question.");
                return;
            }

            // Remove special characters from the question for safe keyword matching
            string questions = RemoveSpecialCharacters(rawQuestion);

            // Display the user's original unmodified message in the chat
            error_method(username, rawQuestion);

            // Clear the input box so the user can type their next message
            question.Clear();

            // Show a "typing..." bubble while the bot prepares its response
            Border typingBorder = AddTypingIndicator();

            // Wait 800 milliseconds to simulate the bot thinking
            await Task.Delay(800);

            // Remove the typing indicator bubble from the chat
            chats.Items.Remove(typingBorder);///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // IMPROVEMENT 1: Check if the user is asking for more info before anything else
            // If yes, elaborate on the last topic instead of doing a normal keyword search
            if (IsFollowUp(questions))
            {
                HandleFollowUp();
                return;
            }

            // Check if the message contains a sentiment word
            // If yes, respond with empathy first then automatically give a related tip
            string detectedSentiment = DetectSentiment(questions);
            if (!string.IsNullOrEmpty(detectedSentiment))
            {
                HandleSentimentWithTip(detectedSentiment, questions);
                return;
            }

            // Check if it is time to remind the user of their saved interests//////////////////////////////////////////////////////////////////////////////////////////
            auto_show_interest();

            // Pass the cleaned question to the AI matching logic
            ai_check(questions);////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        }// end of send

        // -----------------------------------------------------------------------
        // FOLLOW UP METHOD
        // Checks if the user's message is a follow-up request like
        // "tell me more", "explain more", or "give me another tip"
        // Returns true if it is a follow-up, false otherwise
        // -----------------------------------------------------------------------
        private bool IsFollowUp(string input)
        {
            // List of phrases that signal the user wants more info on the last topic
            string[] followUpPhrases = {
                "tell me more",
                "explain more",
                "give me another tip",
                "more info",
                "elaborate",
                "say more",
                "another tip",
                "more details",
                "tell me again",
                "keep going"
            };

            // Check if the user's input contains any of the follow-up phrases
            string lower = input.ToLower();
            foreach (string phrase in followUpPhrases)
            {
                if (lower.Contains(phrase))
                    return true;
            }

            return false;
        }// end of IsFollowUp

        // -----------------------------------------------------------------------
        // HANDLE FOLLOW UP METHOD
        // Called when a follow-up phrase is detected
        // Uses the lastTopic variable to search for more answers on the same topic
        // If no last topic is saved, asks the user what they want more info on
        // -----------------------------------------------------------------------
        private void HandleFollowUp()
        {
            // Check if there is a topic from the previous conversation to elaborate on
            if (!string.IsNullOrEmpty(lastTopic))
            {
                // Tell the user the bot is elaborating on the previous topic
                error_method("ChatBot", "Sure! Here is some more information on " + lastTopic + ":");

                // Search for more answers using the last topic as the keyword
                ai_check(lastTopic);
            }
            else
            {
                // No previous topic found — ask the user what they want to know more about
                error_method("ChatBot", "I'm not sure what topic you'd like more information on. " +
                    "Could you please specify? For example: 'tell me more about phishing'.");
            }
        }// end of HandleFollowUp

        // -----------------------------------------------------------------------
        // DETECT SENTIMENT METHOD
        // Scans the user's message for any known sentiment words
        // Returns the first sentiment word found, or empty string if none found
        // -----------------------------------------------------------------------
        private string DetectSentiment(string input)
        {
            string lower = input.ToLower();

            // Loop through each known sentiment word
            foreach (string sentiment in sentimentWords)
            {
                // If the message contains this sentiment word, return it
                if (lower.Contains(sentiment))
                    return sentiment;
            }

            // No sentiment detected
            return string.Empty;
        }// end of DetectSentiment

        // -----------------------------------------------------------------------
        // HANDLE SENTIMENT WITH TIP METHOD
        // Called when a sentiment is detected in the user's message
        // First runs the normal ai_check to show the empathy response
        // Then automatically follows up with a tip on the related cybersecurity topic
        // The user does NOT need to ask again — the tip is shown automatically
        // -----------------------------------------------------------------------
        private void HandleSentimentWithTip(string sentiment, string fullInput)
        {
            // Run the normal keyword check first to show the empathy response
            // This picks up the sentiment answer from respond.cs
            ai_check(fullInput);

            // Check if this sentiment has a mapped cybersecurity topic
            if (sentimentToTopic.ContainsKey(sentiment))
            {
                // Get the related topic for this sentiment
                string relatedTopic = sentimentToTopic[sentiment];

                // Automatically show a tip on that topic without the user asking
                error_method("ChatBot", "Here is a tip that might help you with that:");

                // Update lastTopic so follow-up questions also work after sentiment
                lastTopic = relatedTopic;

                // Run ai_check on the related topic to get a relevant cybersecurity tip
                ai_check(relatedTopic);
            }
        }// end of HandleSentimentWithTip

        // Typing indicator bubble


        // -----------------------------------------------------------------------
        // ADD TYPING INDICATOR
        // Creates and adds a "typing..." bubble to the chat ListView
        // Returns the Border element so it can be removed after the delay
        // -----------------------------------------------------------------------
        private Border AddTypingIndicator()//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            // Create the outer border container styled the same as a bot message
            Border typingBorder = new Border
            {
                Margin = new Thickness(0, 2, 0, 2),
                Padding = new Thickness(5, 3, 5, 3),
                CornerRadius = new CornerRadius(5),
                Background = new SolidColorBrush(Color.FromRgb(10, 14, 39)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0, 191, 255)),
                BorderThickness = new Thickness(1)
            };

            // Create the text block that will show "ChatBot: typing..."
            TextBlock typingText = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(2),
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 13
            };

            // Add the bold "ChatBot:" label in cyan
            typingText.Inlines.Add(new Run
            {
                Text = "ChatBot: ",
                Foreground = new SolidColorBrush(Color.FromRgb(0, 191, 255)),
                FontWeight = FontWeights.Bold
            });

            // Add the "typing..." text in the standard message color
            typingText.Inlines.Add(new Run
            {
                Text = "typing...",
                Foreground = new SolidColorBrush(Color.FromRgb(200, 220, 255))
            });

            // Place the text inside the border
            typingBorder.Child = typingText;

            // Add the typing bubble to the chat list
            chats.Items.Add(typingBorder);

            // Scroll so the typing indicator is visible
            chats.ScrollIntoView(typingBorder);

            // Return the border so the send method can remove it later
            return typingBorder;

        }// end of AddTypingIndicator


        // -----------------------------------------------------------------------
        // AI CHECK METHOD
        // Receives the cleaned question from the send method
        // Splits it into individual words, skips ignored words,
        // matches keywords against the reply ArrayList, and displays answers
        // -----------------------------------------------------------------------
        private void ai_check(string questions)
        {
            // Check if the question is empty after being cleaned
            if (string.IsNullOrWhiteSpace(questions))
            {
                error_method("ChatBot", "Please enter a valid question.");
                question.Clear();
                return;
            }

            // Double check length after cleaning
            if (questions.Length == 0 || string.IsNullOrWhiteSpace(questions))
            {
                error_method("ChatBot", "I couldn't understand that.");
                question.Clear();
                return;
            }

            // Split the question into individual words using common punctuation as separators
            string[] words = questions.ToLower().Split(
                new char[] { ' ', ',', '.', '?', '!', ';', ':' },
                StringSplitOptions.RemoveEmptyEntries);

            // Tracks whether any keyword match was found
            bool found = false;

            // Builds up the final response message string
            string message = string.Empty;

            // Used to randomly select between multiple matching answers
            Random indexer = new Random();

            // Temporarily stores answers matching the current word being processed
            List<string> per_word = new List<string>();

            // Stores one answer per matched keyword to combine into final response
            List<string> answers_found = new List<string>();


            // Loop through each word extracted from the user's question
            foreach (string word in words)
            {
                // Skip words that are too short or in the ignore list
                if (word.Length < 3 || ignore.Contains(word.ToLower()))
                    continue;

                // Clear per_word list for each new keyword being processed
                per_word.Clear();


                // -----------------------------------------------------------
                // INTEREST DETECTION
                // If the user says they are "interested" in something,
                // extract those topics and save them to a text file
                // -----------------------------------------------------------
                if (word.Contains("interested"))
                {
                    string store_interests = string.Empty;
                    bool found_interest = false;

                    // Use a HashSet to automatically prevent duplicate interests
                    HashSet<string> currentInterests = new HashSet<string>();

                    // Loop through all words in the sentence to find interest topics
                    foreach (string interest in words)
                    {
                        // Clean and normalize each word
                        string clean = interest.ToLower().Trim();
                        clean = Regex.Replace(clean, @"[^a-zA-Z0-9\s]", "");

                        // Only keep meaningful words that are not noise or filler
                        if (!ignore.Contains(clean) && clean != "interested"
                            && clean != "and" && clean != "in" && clean.Length >= 3)
                        {
                            found_interest = true;
                            currentInterests.Add(clean);
                        }
                    }

                    // Combine all found interests into a single comma-separated string
                    store_interests = string.Join(", ", currentInterests);

                    if (found_interest && !string.IsNullOrWhiteSpace(store_interests))
                    {
                        string filename = "interested_topic.txt";
                        bool userFound = false;

                        // Check if the interests file already exists
                        if (File.Exists(filename))
                        {
                            string[] lines = File.ReadAllLines(filename);

                            // Look for an existing entry for this user
                            for (int i = 0; i < lines.Length; i++)
                            {
                                if (lines[i].StartsWith(username))
                                {
                                    userFound = true;

                                    // Extract just the interests portion from the line
                                    string existing = lines[i]
                                        .Replace(username + " interested in:", "")
                                        .ToLower();

                                    // Load existing interests into a HashSet to avoid duplicates
                                    HashSet<string> existingSet = new HashSet<string>(
                                        existing.Split(',')
                                        .Select(x => x.Trim())
                                        .Where(x => x != ""));

                                    // Merge new interests with existing ones
                                    foreach (string item in currentInterests)
                                        existingSet.Add(item);

                                    // Write the merged list back to file
                                    string finalList = string.Join(", ", existingSet);
                                    lines[i] = username + " interested in: " + finalList;
                                    File.WriteAllLines(filename, lines);

                                    message += "Great, I added " + store_interests + " to your interests. ";
                                    break;
                                }
                            }
                        }

                        // If the user has no existing entry, create one
                        if (!userFound)
                        {
                            File.AppendAllText(
                                filename,
                                username + " interested in: " + store_interests + "\n");

                            message += "Great, I will remember that you are interested in " + store_interests + ". ";
                        }
                    }
                    else
                    {
                        // User said "interested" but did not specify a topic
                        message += "Please specify what you are interested in (e.g. 'I am interested in cybersecurity'). ";
                    }

                }// end of interest detection


                // -----------------------------------------------------------
                // KEYWORD MATCHING
                // Search through all stored answers for ones containing the
                // current keyword and collect them into per_word list
                // -----------------------------------------------------------
                bool wordFound = false;

                foreach (string answer in reply)
                {
                    // Check if this answer contains the current keyword
                    if (answer.ToLower().Contains(word))
                    {
                        wordFound = true;
                        per_word.Add(answer);
                    }
                }

                // If matches were found for this word, randomly pick one answer
                if (wordFound && per_word.Count > 0)
                {
                    found = true;////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    // This allows follow-up questions to elaborate on this topic
                    lastTopic = word;

                    // Pick a random answer from all matches for this keyword
                    int indexing = indexer.Next(0, per_word.Count);
                    answers_found.Add(per_word[indexing]);
                }

            }// end of foreach word loop


            // -----------------------------------------------------------
            // DISPLAY RESPONSE
            // If matches were found, display them — otherwise show fallback
            // -----------------------------------------------------------
            if (found && answers_found.Count > 0)
            {
                // Remove any duplicate answers that matched multiple keywords
                answers_found = answers_found.Distinct().ToList();

                // Combine all matched answers into a single response
                foreach (string per_answer in answers_found)
                    message += per_answer + "\n";

                // Display the combined response in the chat
                error_method("ChatBot", message.TrimEnd('\n'));

                // Scroll to the latest message
                chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
            }
            else
            {
                // No keyword matches found — show a random fallback message
                string[] fallbackMessages = {
                    "I'm sorry, I don't understand that. Could you rephrase your question?",
                    "I didn't quite get that. Try asking about cybersecurity topics.",
                    "Hmm, I'm not sure how to respond to that. Can you ask something else?",
                    "I couldn't find an answer for that. Please ask about security or technology.",
                    "My apologies, I don't have information on that topic yet."
                };

                Random random = new Random();
                error_method("ChatBot", fallbackMessages[random.Next(fallbackMessages.Length)]);
            }

            // Clear the input box after processing
            question.Clear();

        }// end of ai_check


        // -----------------------------------------------------------------------
        // REMOVE SPECIAL CHARACTERS METHOD
        // Cleans the user's input before it is processed by ai_check
        // Keeps letters, numbers, spaces, apostrophes and hyphens only
        // -----------------------------------------------------------------------
        private string RemoveSpecialCharacters(string input)
        {
            // Return empty string if input is null or blank
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            StringBuilder sanitized = new StringBuilder();

            // Loop through every character in the input
            foreach (char c in input)
            {
                // Allow letters, digits, whitespace, apostrophes and hyphens
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '\'' || c == '-')
                    sanitized.Append(c);
                else
                    // Replace anything else with a space to keep word separation
                    sanitized.Append(' ');
            }

            // Collapse multiple spaces into one and trim the ends
            string result = sanitized.ToString();
            result = System.Text.RegularExpressions.Regex.Replace(result, @"\s+", " ").Trim();

            return result;

        }// end of RemoveSpecialCharacters


        // -----------------------------------------------------------------------
        // AUTO SHOW INTEREST METHOD
        // Called every time the user sends a message
        // Every 3 messages, it reads the user's saved interests from file
        // and proactively shows related information
        // IsShowingInterest flag prevents recursive loop with ai_check
        // -----------------------------------------------------------------------
        private void auto_show_interest()
        {
            // Check if the counter has reached 3
            if (counting == 3)
            {
                string filename = "interested_topic.txt";

                // Only proceed if the interests file exists
                if (File.Exists(filename))
                {
                    string[] lines = File.ReadAllLines(filename);

                    // Search through all lines for the current user's entry
                    foreach (string line in lines)
                    {
                        if (line.StartsWith(username))
                        {
                            // Find the position of "interested in:" in the line
                            int colonIndex = line.IndexOf("interested in:");

                            if (colonIndex >= 0)
                            {
                                // Extract just the list of interests after the colon
                                string interests = line.Substring(colonIndex + 14).Trim();

                                // Display the interest reminder in the chat
                                error_method("ChatBot",
                                    "Just a reminder, you mentioned you are interested in: " + interests);

                                // Only call ai_check if not already inside it
                                // This prevents the method from calling itself endlessly
                                if (!isShowingInterest)
                                {
                                    isShowingInterest = true;
                                    ai_check(interests);
                                    isShowingInterest = false;
                                }

                                break;
                            }
                        }
                    }
                }

                // Reset the counter back to zero after showing interests
                counting = 0;
            }
            else
            {
                // Increment the counter until it reaches 3
                counting += 1;
            }

        }// end of auto_show_interest


        // -----------------------------------------------------------------------
        // FERROR METHOD (DISPLAY MESSAGE)
        // Displays a formatted chat message bubble in the ListView
        // FIUpdated to use dark navy and cyan theme matching the logo
        // Adds a timestamp in HH:mm format to each message
        // -----------------------------------------------------------------------
        private void error_method(string name, string message)
        {
            // Create the outer border that wraps each chat message
            Border messageBorder = new Border
            {
                Margin = new Thickness(0, 2, 0, 2),
                Padding = new Thickness(5, 3, 5, 3),
                CornerRadius = new CornerRadius(5)
            };

            //  Apply different dark theme colors for bot vs user messages
            if (name.ToLower().Contains("chatbot") || name.ToLower().Contains("chat"))
            {
                // Bot message — dark navy background with cyan border
                messageBorder.Background = new SolidColorBrush(Color.FromRgb(10, 14, 39));
                messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 191, 255));
            }
            else
            {
                // User message — slightly lighter navy with deep blue border
                messageBorder.Background = new SolidColorBrush(Color.FromRgb(15, 22, 58));
                messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(30, 100, 220));
            }

            messageBorder.BorderThickness = new Thickness(1);

            // Create the text block that holds the name label and message content
            TextBlock messageText = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(2),
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 13
            };

            // Cyan for bot name, soft blue for user name
            Brush nameColor = (name.ToLower().Contains("chatbot") || name.ToLower().Contains("chat"))
                ? new SolidColorBrush(Color.FromRgb(0, 191, 255))
                : new SolidColorBrush(Color.FromRgb(100, 160, 255));

            // Off-white blue tint for the message body text
            Brush messageColor = new SolidColorBrush(Color.FromRgb(200, 220, 255));

            // Generate the current time in HH:mm format for the timestamp
            string timestamp = DateTime.Now.ToString("HH:mm");

            // Add the bold sender name label (e.g. "ChatBot: " or "John: ")
            messageText.Inlines.Add(new Run
            {
                Text = name + ": ",
                Foreground = nameColor,
                FontWeight = FontWeights.Bold
            });

            // Add the main message body text
            messageText.Inlines.Add(new Run
            {
                Text = message,
                Foreground = messageColor
            });

            // Add the timestamp in small muted text at the end of the message
            messageText.Inlines.Add(new Run
            {
                Text = "  [" + timestamp + "]",
                Foreground = new SolidColorBrush(Color.FromRgb(70, 100, 150)),
                FontSize = 10
            });

            // Place the text block inside the border
            messageBorder.Child = messageText;

            // Add the completed message bubble to the chat ListView
            chats.Items.Add(messageBorder);

            // Scroll down so the latest message is always visible
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);

        }// end of error_method


    }// end of class
}// end of namespace
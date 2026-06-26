Features Implemented
1. GUI Design and Implementation

Built using WPF (Windows Presentation Foundation)
Three-page layout: Home Page, Username Page and Chat Page
Dark navy #080C1E background with cyan #00BFFF accents throughout
Styled buttons, TextBoxes and chat bubbles matching the app logo aesthetic
Minimum window size enforced to prevent layout breaking on resize
Placeholder hint text in the chat input box
Voice greeting plays automatically on launch using SoundPlayer

2. Keyword Recognition

Recognises cybersecurity-related keywords in user input and responds accordingly
Topics covered include:

2. Keyword Recognition — Recognises cybersecurity topics in user input and responds accordingly. Topics covered include password safety and best practices, phishing scam awareness, general scam detection, firewall and network protection, malware and virus protection, VPN and public Wi-Fi safety, data encryption and HTTPS, ransomware prevention, two-factor authentication, data breach response, social engineering awareness, online privacy protection, financial fraud response, steps to take after being hacked and general cybersecurity information.

3. Random Responses

Every topic has 3 different answers stored in the reply ArrayList
The bot randomly selects one answer each time a keyword is matched
This keeps conversations varied and engaging rather than repetitive
Uses Random and index selection inside ai_check to pick responses
Keywords are included at the start of every answer string so that all 3 options match and randomisation works correctly

4. Conversation Flow

The bot tracks the last topic discussed using the lastTopic variable
When the user types a follow-up phrase, the bot elaborates on the previous topic automatically without the user needing to repeat themselves
Follow-up phrases detected include:

"tell me more"
"explain more"
"give me another tip"
"more info"
"elaborate"
"another tip"
"more details"
"keep going"

5. Memory and Recall

The bot remembers the user's name across sessions using a text file (user_names.txt)
Returning users are greeted differently to new users
The bot remembers topics the user is interested in using a second file (interested_topic.txt)
When the user says "I am interested in [topic]", the interest is saved
Every 3 messages, the bot proactively reminds the user of their saved interests and provides related tips

6. Sentiment Detection — Detects worried, frustrated, confused, curious, angry, sad, scared and nervous sentiments in user messages. When a sentiment is detected the bot first responds with an empathetic message then automatically provides a related cybersecurity tip without the user needing to ask again. Worried users receive a phishing awareness tip, frustrated users receive a password safety tip, confused and curious users receive general cybersecurity information, angry users receive fraud response advice, sad users receive privacy protection tips, scared users receive malware protection advice and nervous users receive VPN and public Wi-Fi safety tips.

7. Error Handling and Edge Cases

Empty input is caught and the user is informed to enter a question
Special characters are removed from input before processing using RemoveSpecialCharacters
Unknown keywords trigger a random fallback message from a list of 5 options
Empty username shows an error message and blocks progression
Username over 20 characters is rejected with an explanation
Application does not crash on any unexpected input

8. Code Structure and Optimisation

Code is split across 5 separate classes for clean organisation
All methods have descriptive names and full comments
ArrayList used for storing answers and ignore words
Dictionary used to map sentiments to cybersecurity topics
HashSet used to prevent duplicate interests being stored
List<string> used for per-word answer matching

demo/
│
├── MainWindow.xaml          — UI layout (Home, Username, Chat pages)
├── MainWindow.xaml.cs       — Main logic (AI matching, sentiment, memory, flow)
├── respond.cs               — All chatbot answers and ignore words list
├── user_name.cs             — Username handling, validation, welcome messages
├── voice_greeting.cs        — Audio greeting on application launch
├── greet.wav                — Voice greeting audio file
├── cybersecurity.jpeg       — App logo image
├── user_names.txt           — Auto-created: stores registered usernames
├── interested_topic.txt     — Auto-created: stores user interests
└── README.md                — Project documentation

Example Conversations

Keyword Recognition:
User:    Tell me about password safety
ChatBot: A strong password should be long and not easy to guess.

Random Responses:
User:    Tell me about phishing        ← asked 3 times
ChatBot: Phishing is a scam where attackers pretend to be trusted sources.
ChatBot: Phishing uses fake messages to trick users into revealing data.
ChatBot: In a phishing attack, attackers use deception to appear legitimate.

Sentiment Detection + Auto Tip:
User:    I am worried about online scams
ChatBot: Don't panic when you're worried, most issues can be fixed quickly.
ChatBot: Here is a tip that might help you with that:
ChatBot: Phishing uses fake messages to trick users into revealing data.

Memory and Recall:
User:    I am interested in privacy
ChatBot: Great, I will remember that you are interested in privacy.
         [3 messages later]
ChatBot: Just a reminder, you mentioned you are interested in: privacy
ChatBot: Adjust your social media privacy settings to control who can see your information.

Conversation Flow:
User:    Tell me about encryption
ChatBot: Encryption converts data into a coded format only authorized parties can read.

User:    Tell me more
ChatBot: Sure! Here is some more information on encryption:
ChatBot: Always ensure websites use HTTPS as it means your connection is encrypted.

Returning User:
User:    John  ← already registered
ChatBot: Hey John, welcome back! How can I help keep you safe online today?

Technical Requirements
IDE: Visual Studio 2019 or later
Framework: .NET Framework 4.7.2 or later
Language: C#
UI: WPF (Windows Presentation Foundation)
OS: Windows (required for WPF and SoundPlayer)

PART 3
# AI Cybersecurity Awareness Chatbot — Part 3 (POE)

A WPF desktop chatbot built in C# for **PROG6221/w — Programming 2A**.
This is the final part of a three-part Portfolio of Evidence. It builds on the Part 1 (console
logic) and Part 2 (GUI conversion) chatbot by adding:

1. **Task Assistant with Reminders**, backed by a **MySQL** database
2. **Cybersecurity Mini-Game (Quiz)** with 12 multiple-choice / true-false questions
3. **NLP Simulation** — keyword-based detection that understands varied phrasing
4. **Activity Log** — a running history of every significant action the bot has taken

All Part 1 / Part 2 features (dynamic responses, keyword recognition, sentiment detection,
follow-up handling, interest tracking) remain fully intact and now live inside a **Chat** tab
alongside three new tabs: **Tasks**, **Quiz**, and **Activity Log**.

---

## Demo video

[Watch the walkthrough video here](PASTE_YOUR_UNLISTED_YOUTUBE_LINK_HERE)

---

## 1. Prerequisites

| Requirement | Notes |
|---|---|
| **Visual Studio 2019 or later** | with the **.NET desktop development** workload (for WPF) installed |
| **.NET Framework 4.7.2** | offered automatically by the VS installer |
| **MySQL Server** (5.7+ or 8.x) | running locally, e.g. via the MySQL Installer, XAMPP, or Docker |
| **NuGet** | used to restore the `MySql.Data` package (built into Visual Studio) |

---

## 2. Setting up the database connection

This project does **not** require you to manually create the database or any tables —
`TaskAssistantDB.cs` does this automatically the first time the application runs.

You only need to tell it how to reach your MySQL server. Open **`TaskAssistantDB.cs`** and edit
the five fields at the top of the class:

```csharp
private static readonly string DbServer   = "localhost";
private static readonly string DbPort     = "3306";
private static readonly string DbName     = "cyberbot_db";   // created automatically
private static readonly string DbUser     = "root";
private static readonly string DbPassword = "";              // set your MySQL password here
```

Once these match your local MySQL setup, just build and run — the `tasks` table is created
automatically on first launch.

> **If the database can't be reached:** the app still runs. The Chat window shows a one-time
> warning explaining the connection error, and the Tasks tab displays the same error. The Task
> Assistant simply won't be able to save anything until the connection is fixed — nothing else in
> the chatbot is affected.

---

## 3. Restoring the MySQL connector package

This project references **MySql.Data** via NuGet.

In Visual Studio:
1. Open the solution.
2. **Tools → NuGet Package Manager → Package Manager Console**
3. Run:
   ```
   Install-Package MySql.Data
   ```
4. **Build → Build Solution** (or press `F5` to build and run).

---

## 4. Project structure

```
demo/
├── App.xaml / App.xaml.cs              Application entry point
├── MainWindow.xaml / .cs                All GUI views + event handling logic
├── respond.cs                           Part 1/2 keyword/answer & ignore-word lists
├── user_name.cs                         Username login/registration logic
├── voice_greeting.cs                    Plays the startup greeting sound
│
├── TaskItem.cs                          Data model for one cybersecurity task
├── TaskAssistantDB.cs                   MySQL CRUD for the Task Assistant (Part 3 / Task 1)
├── QuizQuestion.cs                      Data model for one quiz question
├── QuizGame.cs                          Quiz question bank, scoring & progress (Part 3 / Task 2)
├── NlpProcessor.cs                      Keyword-based NLP simulation (Part 3 / Task 3)
├── ActivityLogger.cs                    Action logging (Part 3 / Task 4)
│
├── Properties/                          AssemblyInfo, Resources, Settings
├── cybersecurity.jpeg / greet.wav       App image and greeting sound
└── demo.csproj / demo.sln               Project & solution files
```

---

## 5. Feature walkthrough

### Chat tab (Parts 1 & 2 + Part 3 commands)
Everything from Parts 1 and 2 still works exactly as before — greetings, cybersecurity topic
keyword matching, sentiment detection with automatic follow-up tips, "tell me more" follow-ups,
and the "interested in..." reminder system.

On top of that, natural-language commands are routed to the new features automatically:

| You type | What happens |
|---|---|
| `Add a task to enable two-factor authentication.` | Adds a task, then asks if you'd like a reminder |
| `Remind me to update my password tomorrow.` | Adds a task **with** a reminder already set |
| `Show my tasks` | Lists all saved tasks with their status and reminders |
| `Start quiz` | Switches to the Quiz tab and starts a new attempt |
| `Show activity log` / `What have you done for me?` | Prints a summary of recent actions |

### Tasks tab (Part 3 / Task 1)
A dedicated panel to add a task (title, description, optional reminder), view all saved tasks,
mark them complete, or delete them — all synced live with the MySQL `tasks` table.

### Quiz tab (Part 3 / Task 2)
12 cybersecurity questions (phishing, passwords, safe browsing, social engineering, firewalls,
ransomware, 2FA), mixing multiple-choice and true/false formats. A random set of 10 is used per
attempt. Each answer gets immediate feedback and a short explanation; score is tracked and a final
encouraging message is shown at the end.

### Activity Log tab (Part 3 / Task 4)
Shows the last 10 actions the bot has taken (tasks added, reminders set, quiz attempts, NLP
commands recognised), each with a timestamp. A **Show Full History** button reveals everything
logged in the current session.

---

## 6. Design notes

- The activity log is stored **in memory** for the current session — it resets when the app is
  closed. Tasks, however, persist permanently in MySQL.
- The NLP simulation uses keyword detection and regular expressions (`string.Contains()`,
  `string.StartsWith()`, `Regex`) rather than a third-party NLP library, as suggested in the brief.
- Username login uses per-session text file storage (unchanged from Part 1/2) — tasks are scoped
  to the logged-in username in the database.

---

## 7. Submission checklist

- [ ] GitHub repository link submitted on ARC
- [ ] Repository includes complete source code, this README, and all project files
- [ ] Minimum **6 commits** with meaningful messages
- [ ] Minimum **3 tags/releases**
- [ ] Unlisted YouTube video link with voice-over, linked above


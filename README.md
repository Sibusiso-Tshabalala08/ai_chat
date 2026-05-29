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

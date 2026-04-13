 AI Cybersecurity Chatbot
A C# console-based chatbot that educates users about cybersecurity topics through an interactive keyword-driven conversation. Built using object-oriented programming principles with .NET Framework.

 Table of Contents

About the Project
Features
Project Structure
Classes Overview
Getting Started
Prerequisites
Setup
How to Use
Cybersecurity Topics Covered
Technologies Used


About the Project
This chatbot was built as a C# learning project that demonstrates object-oriented programming concepts including classes, constructors, access modifiers, loops, and console formatting. It greets the user with an audio message, displays an ASCII art logo, collects the user's name, and then holds a conversation about cybersecurity awareness topics.

Features

 Plays a WAV audio greeting on launch
 Renders a JPEG image as ASCII art in the console
 Collects and validates the user's name
 Keyword-based chatbot covering 12 cybersecurity topics
 Displays a relevant tip after each response
 Colour-formatted console output for a clean user experience
 Two-stage keyword matching — phrase matching first, then word-by-word


Project Structure
ai_chat/
│
├── Program.cs          # Entry point — coordinates all classes
├── playNow.cs          # Plays the WAV audio greeting on startup
├── Logo.cs             # Converts a JPEG image to ASCII art
├── prompt_user.cs      # Collects and validates the user's name
├── chats.cs            # Core chatbot engine with keyword matching
│
├── greeting.wav        # Audio greeting file (place in project root)
└── cybersecurity.jpeg  # Image file for ASCII art (place in project root)

Classes Overview
Program.cs
The entry point of the application. The Main method creates instances of all other classes in the correct order and starts the chatbot conversation.
playNow.cs
Uses a constructor to automatically play a WAV audio file when the program starts. Uses SoundPlayer from System.Media and includes error handling via try-catch.
Logo.cs
Loads a JPEG image using the Bitmap class, resizes it, and maps each pixel's brightness to an ASCII character using a nested for loop. The result is printed to the console as a text-based image.
prompt_user.cs
Displays a welcome banner and collects the user's name using a do-while loop and input validation. Uses console colour formatting to style the output. Exposes a return_name() method so other classes can access the name.
chats.cs
The core chatbot engine. Stores 12 cybersecurity topic entries, each with keywords, a detailed response, and a tip. Uses two-stage matching — first checking for full phrase matches, then falling back to word-by-word matching with stop word filtering.

Getting Started
Prerequisites

Visual Studio (2019 or later recommended)
.NET Framework 4.7.2 or higher
Windows OS (required for SoundPlayer audio playback)

Setup

Clone the repository

bash   git clone https://github.com/your-username/ai-cybersecurity-chatbot.git

Open the solution in Visual Studio by double-clicking the .sln file.
Add the required files to the project root folder (not bin/Debug):

greeting.wav — your audio greeting file
cybersecurity.jpeg — your image for ASCII art


Set both files to copy to output directory:

Right-click each file in Solution Explorer
Select Properties
Set Copy to Output Directory → Copy always


Build and run the project using Ctrl + F5 or the Run button.


How to Use

Run the program — the audio greeting plays and the ASCII logo appears.
Enter your name when prompted.
Type a question or topic related to cybersecurity.
The chatbot will respond with information and a helpful tip.
Type exit to end the conversation.

Example inputs:
What is phishing?
Tell me about passwords
How do I stay safe online?
What is two factor authentication?

Cybersecurity Topics Covered
TopicExample KeywordsPassword safetypassword, passphrase, strong passwordPhishingphishing, scam email, fake emailSafe browsingbrowsing, website, internet safetyMalware & virusesmalware, virus, ransomware, antivirusPrivacy & data protectionprivacy, popia, personal informationSocial media safetysocial media, facebook, instagramOnline bankingbanking, credit card, otpEmail securityemail, spam, attachmentTwo-factor authentication2fa, two factor, authenticationGeneral chatbot infowhat can you help, topics, help me

Technologies Used

Language: C#
Framework: .NET Framework
IDE: Visual Studio
Libraries:

System.Media — audio playback
System.Drawing — image processing for ASCII art
System.Collections — ArrayList for stop words
System.Collections.Generic — List for response entries




Author
Built by Sibusiso Tshabalala
GitHub: @Sibusiso-Tshabalala08

License
This project is open source and available under the MIT License.

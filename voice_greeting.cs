using System;
using System.Media;
using System.IO;

namespace demo
{// start of namespace

    public class voice_greeting
    {// start of class


        // -----------------------------------------------------------------------
        // GREET METHOD
        // Plays the welcome audio file when the application first launches
        // Fixed the file path so it works in both Debug and Release build modes
        // The original used .Replace() which only worked in Debug mode
        // -----------------------------------------------------------------------
        public void greet()
        {// start of greet method

            // Get the directory where the application executable is running from
            // This works whether the app is in bin/Debug or bin/Release
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            // Navigate up two levels from bin/Debug or bin/Release to reach the project root
            // where the greet.wav file is stored
            string projectRoot = Path.GetFullPath(Path.Combine(baseDir, @"..\..\"));

            // Build the full path to the greeting audio file
            string auto_path = Path.Combine(projectRoot, "greet.wav");

            // Only attempt to play if the file actually exists
            // This prevents a crash if the file is missing
            if (File.Exists(auto_path))
            {
                // Create a SoundPlayer instance with the audio file path
                SoundPlayer greetMe = new SoundPlayer(auto_path);

                // Play the greeting sound
                greetMe.Play();
            }

        }// end of greet method


    }// end of class
}// end of namespace
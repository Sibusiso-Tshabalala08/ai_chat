namespace ai_chat
{
    //import the system media
    using System.Media;
    using System;
    using System.Runtime.InteropServices;
    using System.IO;

    public class playNow
    {
        //constructor
        public playNow()
        {
            //now get where the project is
            string project_location = AppDomain.CurrentDomain.BaseDirectory;

            //check if it is geeting the directory
            Console.WriteLine(project_location);
            //lets replace the bin\debug so it can get the audio
            string updated_path = project_location.Replace("bin\\Debug\\", "");
            //combining the wav name as sound.wav with the updated path
            string full_path = Path.Combine(updated_path, "greeting.wav");

            //now lets pass it to the method play_wav
            Play_wav(full_path);

        } //end of constructror

        //method to play sound
        private void Play_wav(string full_path)
        {

            //try n catch
            try
            {
                //or play the sound
                using (SoundPlayer player = new SoundPlayer(full_path))

                {
                    //to play the sound and use this
                    player.PlaySync();
                }


            }
            catch (Exception error)
            {

                //then show the error message
                Console.WriteLine(error.Message);
            }//end of try and catch

        }//end of method play wav

    }//end of class
}// end of namespace
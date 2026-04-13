using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ai_chat
{
    public class Program
    {
        static void Main(string[] args)
        {
            playNow sound = new playNow();


            Logo logo = new Logo();

            {//start main method

                //creating an unstnace  for the class prompt_user
                //with an object name collect_name
                prompt_user collect_name = new prompt_user();

                //call the welcome method
                collect_name.DisplayWelcomeMessage();
                collect_name.asking_name();

                //instance for the class ai_response
                //with an object name chatting 
                chats chatting = new chats();
                chatting.ai_chats(collect_name.return_name());

                //get the returned of the class
            }
        }
    }
}

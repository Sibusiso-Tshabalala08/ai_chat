using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ai_chat
{
    public class Logo
    {
        //constructor
        public Logo()
        {
            //get full path of the image (copied to output directory)
            string path_project = AppDomain.CurrentDomain.BaseDirectory;

            //then replace the bin\\Debug\\
            string new_path_project = path_project.Replace("bin\\Debug\\", "");

            //then combine the prject full path and the image name with the 
            //format
            string full_path = Path.Combine(new_path_project, "cybersecurity.jpeg");

            //then start working on the logo
            //with the Ascii
            Bitmap image = new Bitmap(full_path);
            image = new Bitmap(image, new Size(210, 200));

            //for loop , for inner and outer
            //nested
            for(int height = 0; height < image.Height; height++)
            {
               //then now work on  the width
               for(int width = 0; width < image.Width; width++)
               {
                    //get pixel color and calculate average brightness
                    Color pixelColor = image.GetPixel(width, height);
                    int color = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                    //now make use of the char
                    char ascii_design = color > 200 ? '.' : color > 150 ? '*' : color > 100 ? 'O' : color > 50 ? '#' : '@';
                    Console.Write(ascii_design); //output the design

                }//end of the loop for inner
                Console.WriteLine(); //move to next line after each row
            }//end of the loop outer
        }
    }
}
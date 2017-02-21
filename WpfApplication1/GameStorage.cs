using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.InteropServices;


namespace WpfApplication1
{
    class GameStorage
    {
        //the stack that we will store the players previous data
        Stack<Bitmap> gameData = new Stack<Bitmap>();





        public Bitmap getNextImage()
        {

            return gameData.Pop();
        }

        //store data while the player is playing
        public void storeData(Bitmap inImage)
        {
            gameData.Push(inImage);
        }




    }
}

﻿using DesktopWPFAppLowLevelKeyboardHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.InteropServices;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool startPressed = false;
        private LowLevelKeyboardListener _listener;
        Bitmap bmp;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
                  
            _listener = new LowLevelKeyboardListener();
            _listener.OnKeyPressed += listener_OnKeyPressed;
               
            _listener.HookKeyboard();
            this.textBox_DisplayKeyboardInput.Text = "";


        }

        private void listener_OnKeyPressed(object sender, KeyPressedArgs e)
        {

            if (startPressed)
            {
                this.textBox_DisplayKeyboardInput.Text += e.KeyPressed.ToString();
               
                if (e.KeyPressed.ToString() == "H")
                {
                    System.Console.WriteLine("screenshot!");
                    capture();
                    //take the screenshot
                }

                if (e.KeyPressed.ToString() == "S")
                {
                    System.Console.WriteLine("Trying send the image");
                    sendImage();
                }
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _listener.UnHookKeyboard();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            startPressed = true;
        }

        private void capture()
        {
            //Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            //Graphics graphics = Graphics.FromImage(bitmap as Image);

            //graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            //pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            //pictureBox1.Image = bitmap;
            ////Img1 = bitmap;

            //bitmap.Save("screenshotImage.jpg");
            //bitmap.Save("C:\\temp2.png");

            double screenLeft = SystemParameters.VirtualScreenLeft;
            double screenTop = SystemParameters.VirtualScreenTop;
            double screenWidth = SystemParameters.VirtualScreenWidth;
            double screenHeight = SystemParameters.VirtualScreenHeight;

            using (bmp = new Bitmap((int)screenWidth,
                 (int)screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    //String filename = "ScreenCapture-" + DateTime.Now.ToString("ddMMyyyy-hhmmss") + ".png";
                    Opacity = .0;
                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                    bmp.Save("capturefile.png");///save the image to
                    Opacity = 1;
                }

            }
        }

        private void sendImage()
        {
            byte[] data = new byte[1024];
            int sent;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("128.95.31.226"), 2004); 

            Socket server = new Socket(AddressFamily.InterNetwork,
                            SocketType.Stream, ProtocolType.Tcp);

            try
            {
                server.Connect(ipep);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Unable to connect to server.");
                Console.WriteLine(e.ToString());
                Console.ReadLine();
            }


            //Bitmap bmp = new Bitmap("c:\\eek256.jpg");

            //MemoryStream ms = new MemoryStream();
            // Save to memory using the Jpeg format
            //bmp.Save(ms, ImageFormat.Jpeg);
            MemoryStream memory = new MemoryStream();
            //Bitmap newBitmap = new Bitmap(bmp);
            //bmp.Dispose();
            //bmp = null;
            //do something
            //bmp.Save(memory, ImageFormat.Jpeg);

            //byte[] bytes = ConvertBitMapToByteArray(bmp);
            //byte[] bytes = ImageToByte(bmp);
            byte[] bytes = BitmapToByteArray(bmp);
            //MemoryStream ms = new MemoryStream();
            //bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            // Byte[] bytes = ms.ToArray();

            // read to end
            //byte[] bmpBytes = ms.GetBuffer();
            //bmp.Dispose();
            //ms.Close();

            sent = SendVarData(server, bytes);

            Console.WriteLine("Disconnecting from server...");
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            Console.ReadLine();
        }
        private int SendVarData(Socket s, byte[] data)
        {
            int total = 0;
            int size = data.Length;
            int dataleft = size;
            int sent;

            byte[] datasize = new byte[4];
            datasize = BitConverter.GetBytes(size);
            sent = s.Send(datasize);

            while (total < size)
            {
                sent = s.Send(data, total, dataleft, SocketFlags.None);
                total += sent;
                dataleft -= sent;
            }
            return total;
        }

        public byte[] ConvertBitMapToByteArray(Bitmap bitmap)
        {
            byte[] result = null;
            if (bitmap != null)
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Save(stream, bitmap.RawFormat);
                result = stream.ToArray();
            }
            return result;
        }
        public static byte[] ImageToByte(Bitmap img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        //public static void Save(this BitmapImage image, string filePath)
        //{
        //    BitmapEncoder encoder = new PngBitmapEncoder();
        //    encoder.Frames.Add(BitmapFrame.Create(image));

        //    using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
        //    {
        //        encoder.Save(fileStream);
        //    }
        //}
        public static byte[] BitmapToByteArray(Bitmap bitmap)
        {

            BitmapData bmpdata = null;

            try
            {
                bmpdata = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                int numbytes = bmpdata.Stride * bitmap.Height;
                byte[] bytedata = new byte[numbytes];
                IntPtr ptr = bmpdata.Scan0;

                Marshal.Copy(ptr, bytedata, 0, numbytes);

                return bytedata;
            }
            finally
            {
                if (bmpdata != null)
                    bitmap.UnlockBits(bmpdata);
            }

        }






    }
}

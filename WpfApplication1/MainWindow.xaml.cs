using DesktopWPFAppLowLevelKeyboardHook;
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




namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool startPressed = false;
        private LowLevelKeyboardListener _listener;

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

            using (Bitmap bmp = new Bitmap((int)screenWidth,
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

        //public static void Save(this BitmapImage image, string filePath)
        //{
        //    BitmapEncoder encoder = new PngBitmapEncoder();
        //    encoder.Frames.Add(BitmapFrame.Create(image));

        //    using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
        //    {
        //        encoder.Save(fileStream);
        //    }
        //}







    }
}

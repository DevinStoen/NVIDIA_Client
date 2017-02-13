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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

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
        
        }

        private void listener_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            this.textBox_DisplayKeyboardInput.Text += e.KeyPressed.ToString();

            if(e.KeyPressed.ToString() == "H")
            {
                System.Console.WriteLine("screenshot!");
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _listener.UnHookKeyboard();
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VirtualAssistant.ViewModels;

namespace VirtualAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Thread appThread;
        private ApplicationViewModel assistant;


        public MainWindow()
        {
            InitializeComponent();

            assistant = new ApplicationViewModel();
            assistant.ConsoleWrite += this.OnConsoleWrite;
            assistant.CloseApplication += this.OnApplicationExit;

            // Convert task to background thread
            appThread = new Thread(StartAssistant);
            appThread.IsBackground = true;
            appThread.SetApartmentState(ApartmentState.STA);
            appThread.Start();
        }


        private void StartAssistant(object args)
        {
            assistant.Start();
        }


        private void OnConsoleWrite(object sender, ConsoleWriteEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                this.rtbConsole.AppendText(string.Format("{0}{1}", e.Message, Environment.NewLine));
                this.rtbConsole.ScrollToEnd();
            }));
        }


        private void OnApplicationExit(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                assistant.Dispose();
                appThread.Abort();
                appThread = null;
                this.Close();
            }));
        }
    }
}

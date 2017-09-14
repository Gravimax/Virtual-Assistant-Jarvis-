using System;
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
        public MainWindow()
        {
            InitializeComponent();
            ApplicationViewModel assistant = new ApplicationViewModel();
            assistant.ConsoleWrite += this.OnConsoleWrite;
            assistant.CloseApplication += this.OnApplicationExit;

            // Convert task to background thread
            Task.Run(() =>
            {
                assistant.Start();
            });
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
                this.Close();
            }));
        }
    }
}

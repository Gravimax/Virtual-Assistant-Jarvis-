using System;
using System.ComponentModel;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;
using System.Windows;
using VirtualAssistant.ViewModels;

namespace VirtualAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool _isSpeech;

        public bool IsSpeech
        {
            get { return _isSpeech; }
            set
            {
                _isSpeech = value;
                OnPropertyChanged("IsSpeech");
            }
        }


        private Thread appThread;
        private ApplicationViewModel assistant;


        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            assistant = new ApplicationViewModel();
            assistant.ConsoleWrite += this.OnConsoleWrite;
            assistant.CloseApplication += this.OnApplicationExit;
            assistant.SpeechDetected += this.SpeechRecognitionEngine_SpeechDetected;
            assistant.SpeechRecognized += this.engine_SpeechRecognized;
            assistant.SpeechRecognitionRejected += this.SpeechRecognitionEngine_SpeechRecognitionRejected;

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


        private void SpeechRecognitionEngine_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                IsSpeech = true;
            }));
        }


        private void engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                IsSpeech = false;
            }));
        }


        private void SpeechRecognitionEngine_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                IsSpeech = false;
            }));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

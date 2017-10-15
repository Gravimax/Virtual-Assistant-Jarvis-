using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using VirtualAssistant.CommandProcessing;
using VirtualAssistant.CommandProcessing.Parsers;
using VirtualAssistant.Models;

namespace VirtualAssistant.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Properties


        SpeechRecognitionEngine speechRecognitionEngine = null;

        SpeechSynthesizer speechSynthesizer = null;

        List<ICommand> commandList = new List<ICommand>();
        List<string> commandWords = new List<string>();

        ApplicationConfiguation appConfig;

        #endregion

        public DelegateCommand EditCommandsCommand { get; private set; }


        #region ctor

        public ApplicationViewModel()
        {
            EditCommandsCommand = new DelegateCommand(EditCommands);

            commandList.Add(new WeatherCommandParser());
            commandList.Add(new DateTimeCommandParser());
            commandList.Add(new OpenBrowserCommandParser());
            commandList.Add(new CloseApplicationCommandParser());
            commandList.Add(new ConditionParser());
        }

        #endregion


        #region Start Process


        public void Start()
        {
            try
            {
                appConfig = Utilities.LoadApplicationConfig();

                // Create the engine
                speechRecognitionEngine = CreateSpeechEngine("en-US");

                // Hook to events
                speechRecognitionEngine.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(SpeechRecognitionEngine_SpeechDetected);
                speechRecognitionEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(engine_SpeechRecognized);
                speechRecognitionEngine.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(SpeechRecognitionEngine_SpeechRecognitionRejected);

                // Load dictionary
                LoadGrammerAndCommands();

                // Start listening
                speechRecognitionEngine.SetInputToDefaultAudioDevice();
                speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);

                // Create the speech synthesizer
                speechSynthesizer = new SpeechSynthesizer();

                foreach (var item in speechSynthesizer.GetInstalledVoices().Select(v => v.VoiceInfo))
                {
                    OnConsoleWrite(string.Format("Name: {0}, Gender: {1}, Age: {2}", item.Description, item.Gender, item.Age));
                }

                speechSynthesizer.SetOutputToDefaultAudioDevice();
                speechSynthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);

                speechSynthesizer.SpeakStarted += OnSpeakStarted;
                speechSynthesizer.SpeakProgress += OnSpeakProgress;
                speechSynthesizer.SpeakCompleted += OnSpeakCompleted;
            }
            catch (Exception ex)
            {
                EventLogging.EventLogger.WriteEventLog("", ex);
                OnConsoleWrite("Voice recognition failed " + ex.Message);
            }
        }


        #endregion


        #region Delegate Commands


        private void EditCommands(object args)
        {

        }


        #endregion


        #region Speech Engine


        private SpeechRecognitionEngine CreateSpeechEngine(string preferredCulture)
        {
            SpeechRecognitionEngine speechRecognitionEngine = null;

            foreach (RecognizerInfo config in SpeechRecognitionEngine.InstalledRecognizers())
            {
                if (config.Culture.ToString() == preferredCulture)
                {
                    speechRecognitionEngine = new SpeechRecognitionEngine(config);
                }
            }

            // If the desired culture is not installed, then load default
            if (speechRecognitionEngine == null)
            {
                OnConsoleWrite("The desired culture is not installed on this machine, the speech-engine will continue using" +
                    SpeechRecognitionEngine.InstalledRecognizers()[0].Culture.ToString() +
                    " as the default culture.");
                speechRecognitionEngine = new SpeechRecognitionEngine();
            }

            return speechRecognitionEngine;
        }


        private void LoadGrammerAndCommands()
        {
            try
            {
                Choices texts = new Choices();
                texts.Add(appConfig.AssistantName);

                List<string> tempCommandWords = new List<string>();

                foreach (var item in commandList)
                {
                    tempCommandWords.AddRange(item.CommandList);
                }

                commandWords = tempCommandWords.Distinct().ToList(); // Get a unique list of command words

                DictationGrammar dict = new DictationGrammar();
                speechRecognitionEngine.LoadGrammar(dict);
            }
            catch (Exception ex)
            {
                EventLogging.EventLogger.WriteEventLog("", ex);
                throw ex;
            }
        }


        private string GetKknownTextOrExecute(string command)
        {
            if (command.Contains(appConfig.AssistantName))
            {
                if (command.StartsWith(appConfig.AssistantName) || command.EndsWith(appConfig.AssistantName))
                {
                    command = command.Replace(appConfig.AssistantName, "").Trim();
                }
                else
                {
                    // Command is too complex for us to process right now.
                    OnConsoleWrite("Command too complex.", true);
                    return null;
                }
            }
            else
            {
                OnConsoleWrite("Unnkown command: " + command, true);
                return null;
            }

            try
            {
                command = command.ToLower();

                ICommand commandX = GetCommandToProcess(command);

                if (commandX is CloseApplicationCommandParser)
                {
                    OnCloseApplication();
                    return null;
                }

                ICommandInstance instance = commandX.GetCommandInstance();
                ReturnResult result = instance.RunCommand(command);

                if (result.HasDisplay)
                {
                    // display something here: html, text, etc.?
                }

                return result.Response;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                OnConsoleWrite("Exception processing command: " + command, true);
                EventLogging.EventLogger.WriteEventLog("", ex);

                return "There was an error processing your command";
            }
        }


        private ICommand GetCommandToProcess(string command)
        {
            List<string> temp = new List<string>();

            foreach (var item in commandWords)
            {
                if (command.Contains(item)) { temp.Add(item); }
            }

            if (temp.Count > 0)
            {
                List<ICommand> tempx = new List<ICommand>();
                foreach (var item in commandList)
                {
                    if (temp.All(word => item.CommandList.Contains(word)))
                    {
                        tempx.Add(item);
                    }
                }

                if (tempx.Count == 0)
                {
                    return new UnknownCommandParser("Unknown command");
                }
                if (tempx.Count == 1) // There should only be one ICommand
                {
                    return tempx.First();
                }
                else
                {
                    // If there's more than one then the user has to be more specific
                    return new UnknownCommandParser("Command not specific enough");
                }
            }
            else
            {
                return new UnknownCommandParser("Unknown command");
            }
        }


        #endregion


        public event SpeechDetectedEventHandler SpeechDetected;

        private void SpeechRecognitionEngine_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            SpeechDetected?.Invoke(sender, e);
        }


        public event SpeechRecognizedEventHandler SpeechRecognized;

        private void engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string response = GetKknownTextOrExecute(e.Result.Text);

            if (!string.IsNullOrEmpty(response))
            {
                OnConsoleWrite(response);
                speechSynthesizer.SpeakAsync(response);
            }

            SpeechRecognized?.Invoke(sender, e);
        }


        public event SpeechRecognitionRejectedEventHandler SpeechRecognitionRejected;

        private void SpeechRecognitionEngine_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            SpeechRecognitionRejected?.Invoke(sender, e);
        }


        public event SpeakStartedEventHandler SpeakStarted;

        private void OnSpeakStarted(object sender, SpeakStartedEventArgs e)
        {
            SpeakStarted?.Invoke(sender, e);   
        }


        public event SpeakProgressEventHandler SpeakProgress;

        private void OnSpeakProgress(object sender, SpeakProgressEventArgs e)
        {
            SpeakProgress?.Invoke(sender, e);
        }


        public event SpeakCompletedEventHandler SpeakCompleted;

        private void OnSpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            SpeakCompleted?.Invoke(sender, e);
        }


        public event ConsoleWriteEventHandler ConsoleWrite;

        private void OnConsoleWrite(string message, bool isError = false)
        {
            ConsoleWrite?.Invoke(this, new ConsoleWriteEventArgs(message, isError));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public event ApplicationCloseEventHandler CloseApplication;

        private void OnCloseApplication()
        {
            CloseApplication?.Invoke(this, EventArgs.Empty);
        }


        #region Dispose

        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        System.Runtime.InteropServices.SafeHandle handle = new Microsoft.Win32.SafeHandles.SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                speechRecognitionEngine.Dispose();
                speechRecognitionEngine = null;
                speechSynthesizer.Dispose();
                speechSynthesizer = null;
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        #endregion
    }
}

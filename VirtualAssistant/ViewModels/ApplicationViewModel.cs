using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Diagnostics;
using VirtualAssistant.Models;
using VirtualAssistant.InternalCommands;
using System.ComponentModel;

namespace VirtualAssistant.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Properties


        SpeechRecognitionEngine speechRecognitionEngine = null;

        SpeechSynthesizer speechSynthesizer = null;

        List<CommandItem> commands = new List<CommandItem>();

        string lastCommand = "";

        ApplicationConfiguation appConfig;

        #endregion

        public DelegateCommand EditCommandsCommand { get; private set; }


        #region ctor

        public ApplicationViewModel()
        {
            EditCommandsCommand = new DelegateCommand(EditCommands);
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

                // Hook to event
                speechRecognitionEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(engine_SpeechRecognized);
                
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
                speechSynthesizer.SpeakStarted += SpeechSynthesizer_SpeakStarted;
                speechSynthesizer.SpeakProgress += SpeechSynthesizer_SpeakProgress;
                speechSynthesizer.SpeakCompleted += SpeechSynthesizer_SpeakCompleted;
            }
            catch (Exception ex)
            {
                OnConsoleWrite("Voice recognition failed " + ex.Message);
            }
        }


        private void SpeechSynthesizer_SpeakStarted(object sender, SpeakStartedEventArgs e)
        {
            // Nothing now
        }


        private void SpeechSynthesizer_SpeakProgress(object sender, SpeakProgressEventArgs e)
        {
            // Nothing now
        }


        private void SpeechSynthesizer_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            // Nothing now
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

                commands = Utilities.LoadCommands();

                if (commands.Count == 0)
                {
                    // It's new so create a default list
                    commands.Add(new CommandItem { Commands = new List<string>() { "hello" }, Response = "Good day", AppendName = AppendNameType.Default });
                    commands.Add(new CommandItem { Commands = new List<string>() { "how are you" }, Response = "I'm super, thanks for asking", AppendName = AppendNameType.No });
                    commands.Add(new CommandItem { Commands = new List<string>() { "close assistant", "close virtual assistant" }, Response = "Closing", AppendName = AppendNameType.No, CommandType = VirtualCommandType.EventCommand });
                    commands.Add(new CommandItem { Commands = new List<string>() { "open browser", "open chrome" }, Response = "", Target = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe", AppendName = AppendNameType.No, CommandType = VirtualCommandType.ShellCommand });
                    commands.Add(new CommandItem { Commands = new List<string>() { "open slash" }, Response = "", Target = "https://slashdot.org/", AppendName = AppendNameType.No, CommandType = VirtualCommandType.ShellCommand });
                    commands.Add(new CommandItem { Commands = new List<string>() { "what day is it", "what time is it", "what is the time" }, CommandTarget = "VirtualAssistant.InternalCommands.GetDate", AppendName = AppendNameType.No, CommandType = VirtualCommandType.InternalCommand });
                    commands.Add(new CommandItem { Commands = new List<string>() { "how's the weather",
                        "what's the weather like",
                        "what's it like outside",
                        "what will tomorrow be like",
                        "what's tomorrows forecast",
                        "what's tomorrow like",
                        "what's the temperature",
                        "what's the temperature outside"}, CommandArgs = "2389166/dana point, ca/f", CommandTarget = "VirtualAssistant.InternalCommands.GetWeather", AppendName = AppendNameType.No,
                        CommandType = VirtualCommandType.InternalCommand
                    });

                    Utilities.SaveCommands(commands);
                }

                foreach (var item in commands)
                {
                    // Add the text to the known choices of speech engine
                    foreach (var command in item.Commands)
                    {
                        texts.Add(command);
                    }
                }

                Grammar wordsList = new Grammar(new GrammarBuilder(texts));
                speechRecognitionEngine.LoadGrammar(wordsList);

                DictationGrammar dict = new DictationGrammar();
                speechRecognitionEngine.LoadGrammar(dict);
            }
            catch (Exception ex)
            {
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
                    // Command is too complex for us to process righy now.
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

                var cmd = commands.FirstOrDefault(x => x.Commands.Contains(command.ToLower()));

                if (cmd != null)
                {
                    OnConsoleWrite("Recognize command: " + command);

                    if (cmd.IsShellCommand)
                    {
                        Process proc = new Process();
                        proc.EnableRaisingEvents = false;
                        proc.StartInfo.FileName = cmd.Target;
                        if (cmd.CommandArgs != null)
                        {
                            proc.StartInfo.Arguments = cmd.CommandArgs;
                        }

                        proc.Start();
                        lastCommand = command;

                        return !string.IsNullOrEmpty(cmd.Response) ? cmd.Response + ((cmd.AppendName == AppendNameType.Yes || cmd.AppendName == AppendNameType.Default) ? " " + appConfig.GetName() : "") : null;
                    }
                    else if (cmd.IsInternalCommand)
                    {
                        lastCommand = command;
                        Type t = Type.GetType(cmd.CommandTarget);
                        var cmmd = (IInternalCommand)Activator.CreateInstance(t);
                        ReturnResult result = cmmd.RunCommand(cmd, command);

                        if (result.HasDisplay)
                        {
                            // display something here: html, text, etc.
                        }

                        return result.Response;
                    }
                    else if (cmd.IsEventCommand)
                    {
                        OnCloseApplication();
                        return cmd.Response;
                    }
                    else
                    {
                        lastCommand = command;
                        return cmd.Response + ((cmd.AppendName == AppendNameType.Yes || cmd.AppendName == AppendNameType.Default) ? " " + appConfig.GetName() : "");
                    }
                }
                else
                {
                    OnConsoleWrite("Unnkown command: " + command, true);
                    return null;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                OnConsoleWrite("Exception processing command: " + command, true);
                lastCommand = command;
                return "There was an error processing your command";
            }
        }


        private void engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string response = GetKknownTextOrExecute(e.Result.Text);

            if (!string.IsNullOrEmpty(response))
            {
                speechSynthesizer.SpeakAsync(response);
            }
        }


        #endregion


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


        public event ApplicationClose CloseApplication;

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
                speechRecognitionEngine = null;
                speechSynthesizer = null;
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        #endregion
    }
}

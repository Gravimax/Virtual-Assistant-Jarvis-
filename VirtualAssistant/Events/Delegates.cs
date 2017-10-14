using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistant
{
    public delegate void ConsoleWriteEventHandler(object sender, ConsoleWriteEventArgs e);
    public delegate void ApplicationCloseEventHandler(object sender, EventArgs e);
    public delegate void SpeakStartedEventHandler(object sender, SpeakStartedEventArgs e);
    public delegate void SpeakProgressEventHandler(object sender, SpeakProgressEventArgs e);
    public delegate void SpeakCompletedEventHandler(object sender, SpeakCompletedEventArgs e);

    public delegate void SpeechDetectedEventHandler(object sender, SpeechDetectedEventArgs e);
    public delegate void SpeechRecognizedEventHandler(object sender, SpeechRecognizedEventArgs e);
    public delegate void SpeechRecognitionRejectedEventHandler(object sender, SpeechRecognitionRejectedEventArgs e);
}

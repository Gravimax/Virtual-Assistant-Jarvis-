using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace VirtualAssistant.Models
{
    [Serializable]
    public class CommandItem : INotifyPropertyChanged
    {
        private string _command;
        [XmlElement(DataType = "string", ElementName = "Command")]
        public string Command
        {
            get { return _command; }
            set
            {
                if (_command != value)
                {
                    _command = value;
                    OnPropertyChanged("Command");
                }
            }
        }

        private List<string> _Commands;
        [XmlArray(ElementName = "Commands")]
        public List<string> Commands
        {
            get { return _Commands; }
            set
            {
                if (_Commands != value)
                {
                    _Commands = value;
                    OnPropertyChanged("Commands");
                }
            }
        }

        private string _response;
        [XmlElement(DataType = "string", ElementName = "Response")]
        public string Response
        {
            get { return _response; }
            set
            {
                if (_response != value)
                {
                    _response = value;
                    OnPropertyChanged("Response");
                }
            }
        }

        private string _target;
        [XmlElement(DataType = "string", ElementName = "Target")]
        public string Target
        {
            get { return _target; }
            set
            {
                if (_target != value)
                {
                    _target = value;
                    OnPropertyChanged("Target");
                }
            }
        }

        private string _commandArgs;
        [XmlElement(DataType = "string", ElementName = "CommandArgs")]
        public string CommandArgs
        {
            get { return _commandArgs; }
            set
            {
                if (_commandArgs != value)
                {
                    _commandArgs = value;
                    OnPropertyChanged("CommandArgs");
                }
            }
        }

        private string _commandTarget;
        [XmlElement(DataType = "string", ElementName = "CommandTarget")]
        public string CommandTarget
        {
            get { return _commandTarget; }
            set
            {
                if (_commandTarget != value)
                {
                    _commandTarget = value;
                    OnPropertyChanged("CommandTarget");
                }
            }
        }

        // 1 - yes, 2 - no, 3 - app default
        private AppendNameType _appendName;
        [XmlElement(ElementName = "AppendName")]
        public AppendNameType AppendName
        {
            get { return _appendName; }
            set
            {
                if (_appendName != value)
                {
                    _appendName = value;
                    OnPropertyChanged("AppendName");
                }
            }
        }

        private VirtualCommandType _commandType = VirtualCommandType.Normal;
        [XmlElement(ElementName = "CommandType")]
        public VirtualCommandType CommandType
        {
            get { return _commandType; }
            set
            {
                if (_commandType != value)
                {
                    _commandType = value;
                    OnPropertyChanged("CommandType");
                }
            }
        }


        public bool IsInternalCommand
        {
            get { return CommandType == VirtualCommandType.InternalCommand; }
        }


        public bool IsShellCommand
        {
            get { return CommandType == VirtualCommandType.ShellCommand; }
        }


        public bool IsEventCommand
        {
            get { return CommandType == VirtualCommandType.EventCommand; }
        }

        public bool Valedate(out string errorList)
        {
            bool isValid = true;
            string errors = "";

            if (IsInternalCommand & IsShellCommand)
            {
                errors += "A command item cannot be both an internal command and a shell command.\r\n";
                isValid = false;
            }

            if (string.IsNullOrEmpty(Command))
            {
                errors += "A command is required.\r\n";
                isValid = false;
            }

            if (!IsInternalCommand & !IsShellCommand & string.IsNullOrEmpty(Response))
            {
                errors += "A command requires a response.\r\n";
                isValid = false;
            }

            errorList = errors;
            return isValid;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

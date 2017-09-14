using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace VirtualAssistant.Models
{
    [Serializable]
    public class ApplicationConfiguation : INotifyPropertyChanged
    {
        private string _firstName;
        [XmlElement(DataType = "string", ElementName = "FirstName")]
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        private string _lastName;
        [XmlElement(DataType = "string", ElementName = "LastName")]
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        private string _suffix;
        [XmlElement(DataType = "string", ElementName = "Suffix")]
        public string Suffix
        {
            get { return _suffix; }
            set
            {
                if (_suffix != value)
                {
                    _suffix = value;
                    OnPropertyChanged("Suffix");
                }
            }
        }

        private bool _useSuffix;
        [XmlElement(DataType = "boolean", ElementName = "UseSuffix")]
        public bool UseSuffix
        {
            get { return _useSuffix; }
            set
            {
                if (_useSuffix != value)
                {
                    _useSuffix = value;
                    OnPropertyChanged("UseSuffix");
                }
            }
        }

        private bool _useNameInResponse;
        [XmlElement(DataType = "boolean", ElementName = "UseNameInResponse")]
        public bool UseNameInResponse
        {
            get { return _useNameInResponse; }
            set
            {
                if (_useNameInResponse != value)
                {
                    _useNameInResponse = value;
                    OnPropertyChanged("UseNameInResponse");
                }
            }
        }

        private string _zipCode;
        [XmlElement(DataType = "string", ElementName = "ZipCode")]
        public string ZipCode
        {
            get { return _zipCode; }
            set
            {
                if (_zipCode != value)
                {
                    _zipCode = value;
                    OnPropertyChanged("ZipCode");
                }
            }
        }

        private string __assistantName;
        [XmlElement(DataType = "string", ElementName = "AssistantName")]
        public string AssistantName
        {
            get { return __assistantName; }
            set
            {
                if (__assistantName != value)
                {
                    __assistantName = value;
                    OnPropertyChanged("AssistantName");
                }
            }
        }

        public string GetName()
        {
            return UseNameInResponse ? (UseSuffix ? Suffix : FirstName) : null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

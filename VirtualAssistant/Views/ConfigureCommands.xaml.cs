using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using VirtualAssistant.Models;

namespace VirtualAssistant.Views
{
    /// <summary>
    /// Interaction logic for ConfigureCommands.xaml
    /// </summary>
    public partial class ConfigureCommands : Window, INotifyPropertyChanged
    {
        private List<CommandItem> _commands;

        public List<CommandItem> Commands
        {
            get { return _commands; }
            set
            {
                _commands = value;
                OnPropertyChanged("Commands");
            }
        }

        public ConfigureCommands(List<CommandItem> commands)
        {
            InitializeComponent();
            Commands = commands;
            this.DataContext = this;
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

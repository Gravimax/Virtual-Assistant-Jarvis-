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
    /// Interaction logic for ConfigureCommand.xaml
    /// </summary>
    public partial class ConfigureCommand : Window, INotifyPropertyChanged
    {
        private CommandItem _command;

        public CommandItem Command
        {
            get { return _command; }
            set
            {
                _command = value;
                OnPropertyChanged("Command");
            }
        }

        private List<GenericComboItem> _selectionList;

        public List<GenericComboItem> SelectionList
        {
            get { return _selectionList; }
            set
            {
                _selectionList = value;
                OnPropertyChanged("SelectionList");
            }
        }

        private CommandItem commandItem;

        public ConfigureCommand(CommandItem commandItem)
        {
            InitializeComponent();
            this.commandItem = commandItem;

            Command.AppendName = commandItem.AppendName;
            Command.Command = commandItem.Command;
            Command.CommandTarget = commandItem.CommandTarget;

            Command.Response = commandItem.Response;
            Command.Target = commandItem.Target;

            SelectionList.Add(new GenericComboItem { Id = 1, Value = "Yes" });
            SelectionList.Add(new GenericComboItem { Id = 2, Value = "No" });
            SelectionList.Add(new GenericComboItem { Id = 3, Value = "Application Default" });

            this.DataContext = this;
        }

        private void btnTargetSelect_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

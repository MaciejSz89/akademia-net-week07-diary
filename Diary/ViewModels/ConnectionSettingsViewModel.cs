using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Diary.Commands;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Diary.ViewModels
{
    public class ConnectionSettingsViewModel : ViewModelBase
    {

        public ConnectionSettingsViewModel()
        {
            ConfirmCommand = new AsyncRelayCommand(Confirm);
            CloseCommand = new RelayCommand(Close);

            LoadConnectionSettings();
        }

        private void LoadConnectionSettings()
        {
            ServerName = Properties.Settings.Default.ServerName;
            DatabaseName = Properties.Settings.Default.DatabaseName;
            UserId = Properties.Settings.Default.UserId;
            Password = Properties.Settings.Default.Password;


        }

        private void Close(object obj)
        {
            CloseWindow(obj as Window);
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }

        private async Task Confirm(object obj)
        {
            var connectionString = $"Server={ServerName};User Id={UserId};Password={Password};";

            if (Repository.IsConnectionAvailable(connectionString))
            {
                SaveConnectionSettings();
                Repository.RestartApplication();
            }
            else
            {
                await DisplayConnectionSettingsErrorMessage(obj);
            }
        }

        private static async Task DisplayConnectionSettingsErrorMessage(object obj)
        {
            var metroConnectionWindow = obj as MetroWindow;
            var metroMainWindow = Application.Current.MainWindow as MetroWindow;
            var height = metroConnectionWindow.Height;
            var width = metroConnectionWindow.Width;
            var left = metroConnectionWindow.Left;
            var top = metroConnectionWindow.Top;


            metroConnectionWindow.Height = metroMainWindow.Height;
            metroConnectionWindow.Width = metroMainWindow.Width;
            metroConnectionWindow.Left = metroMainWindow.Left;
            metroConnectionWindow.Top = metroMainWindow.Top;
            var dialog = await metroConnectionWindow.ShowMessageAsync("Błąd danych", "Nie udało się nawiązać połączenia z podanymi ustawieniami. Popraw dane lub sprawdź działanie serwera.", MessageDialogStyle.Affirmative);


            if (dialog == MessageDialogResult.Affirmative)
            {
                metroConnectionWindow.Height = height;
                metroConnectionWindow.Width = width;
                metroConnectionWindow.Left = left;
                metroConnectionWindow.Top = top;

            }
        }

        private void SaveConnectionSettings()
        {
            Properties.Settings.Default.ServerName = ServerName;
            Properties.Settings.Default.DatabaseName = DatabaseName;
            Properties.Settings.Default.UserId = UserId;
            Properties.Settings.Default.Password = Password;
            Properties.Settings.Default.Save();
        }



        public ICommand ConfirmCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        private string _serverName;

        public string ServerName
        {
            get { return _serverName; }
            set
            {
                _serverName = value;
                OnPropertyChanged();
            }
        }

        private string _databaseName;

        public string DatabaseName
        {
            get { return _databaseName; }
            set
            {
                _databaseName = value;
                OnPropertyChanged();
            }
        }

        private string _userId;

        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChanged();
            }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;

            }
        }



    }


}

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

        private ConnectionSettings _connectionSettings;
        private readonly bool _canCloseWindow;

        public ConnectionSettings ConnectionSettings
        {
            get { return _connectionSettings; }
            set
            {
                _connectionSettings = value;
                OnPropertyChanged();
            }
        }

        public ConnectionSettingsViewModel(bool canCloseWindow)
        {

            ConnectionSettings = new ConnectionSettings();

            ConfirmCommand = new AsyncRelayCommand(Confirm, CanConfirm);
            CloseCommand = new RelayCommand(Close);
            _canCloseWindow = canCloseWindow;
        }

        private bool CanConfirm(object arg)
        {
            return ConnectionSettings.IsValid;
        }

        private void Close(object obj)
        {
            if (_canCloseWindow)
                CloseWindow(obj as Window);
            else
                Application.Current.Shutdown();
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }

        private async Task Confirm(object obj)
        {

            var settingsSaved = ConnectionSettings.Save();

            if (settingsSaved)
            {
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




        public ICommand ConfirmCommand { get; set; }
        public ICommand CloseCommand { get; set; }





    }


}

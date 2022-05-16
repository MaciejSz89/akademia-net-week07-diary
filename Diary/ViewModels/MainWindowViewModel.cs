using Diary.Commands;
using Diary.Models;
using Diary.Models.Domains;
using Diary.Models.Wrappers;
using Diary.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ConnectionSettings _connectionSettings;
        public MainWindowViewModel()
        {
            _connectionSettings = new ConnectionSettings();
            RefreshStudentsCommand = new RelayCommand(RefreshStudents);
            AddStudentCommand = new RelayCommand(AddEditStudent);
            EditStudentCommand = new RelayCommand(AddEditStudent, CanEditDeleteStudent);
            DeleteStudentCommand = new AsyncRelayCommand(DeleteStudents, CanEditDeleteStudent);
            ConnectionSettingsCommand = new RelayCommand(ConnectionSettings);
            LoadedWindowCommand = new RelayCommand(LoadedWindow);

        }

        private async void LoadedWindow(object obj)
        {
            if (!_connectionSettings.IsConnectionAvailable())
            {
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                var dialog = await metroWindow.ShowMessageAsync("Nie można nawiązać połączenia z bazą danych", "Czy chcesz poprawić ustawienia połączenia?", MessageDialogStyle.AffirmativeAndNegative);
                if (dialog == MessageDialogResult.Affirmative)
                {
                    var connectionSettingsView = new ConnectionSettingsView(false);
                    connectionSettingsView.ShowDialog();
                }
                else if (dialog == MessageDialogResult.Negative)
                {
                    Application.Current.Shutdown();
                }
            }
            else
            {
                RefreshDiary();
                InitGroups();
            }


        }

        private async Task DisplayConnectionErrorMessage()
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            var dialog = await metroWindow.ShowMessageAsync("Nie można nawiązać połączenia z bazą danych", "Czy chcesz poprawić ustawienia połączenia?", MessageDialogStyle.AffirmativeAndNegative);
            if (dialog == MessageDialogResult.Affirmative)
            {
                var connectionSettingsView = new ConnectionSettingsView(true);
                connectionSettingsView.ShowDialog();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private ObservableCollection<StudentWrapper> _students;
        public ObservableCollection<StudentWrapper> Students
        {
            get
            {
                return _students;
            }
            set
            {
                _students = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Group> _groups;

        public ObservableCollection<Group> Groups
        {
            get
            {
                return _groups;
            }
            set
            {
                _groups = value;
                OnPropertyChanged();
            }
        }

        private Repository _repository = new Repository();

        private int _selectedGroupId;

        public int SelectedGroupId
        {
            get
            {
                return _selectedGroupId;
            }
            set
            {
                _selectedGroupId = value;
                RefreshDiary();
                OnPropertyChanged();
            }
        }

        private StudentWrapper _selectedStudent;

        public StudentWrapper SelectedStudent
        {
            get
            {
                return _selectedStudent;
            }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshStudentsCommand { get; set; }
        public ICommand AddStudentCommand { get; set; }
        public ICommand EditStudentCommand { get; set; }
        public ICommand DeleteStudentCommand { get; set; }
        public ICommand ConnectionSettingsCommand { get; set; }
        public ICommand LoadedWindowCommand { get; set; }

        public void RefreshStudents(object obj)
        {
            RefreshDiary();
        }
        async private Task DeleteStudents(object obj)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            var dialog = await metroWindow.ShowMessageAsync("Usuwanie ucznia", $"Czy na pewno chcesz usunąć ucznia {SelectedStudent.FirstName} {SelectedStudent.LastName}?", MessageDialogStyle.AffirmativeAndNegative);
            if (dialog != MessageDialogResult.Affirmative)
                return;


            _repository.DeleteStudent(SelectedStudent.Id);

            RefreshDiary();
        }

        private bool CanEditDeleteStudent(object obj)
        {
            return SelectedStudent != null;
        }


        private void AddEditStudent(object obj)
        {
            var addEditStudentWindow = new AddEditStudentView(obj as StudentWrapper);
            addEditStudentWindow.Closed += AddEditStudentWindow_Closed;
            addEditStudentWindow.ShowDialog();
        }

        private void AddEditStudentWindow_Closed(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void InitGroups()
        {

            var groups = _repository.GetGroups();
            groups.Insert(0, new Group { Id = 0, Name = "Wszystkie" });

            Groups = new ObservableCollection<Group>(groups);

            SelectedGroupId = 0;
        }

        private void RefreshDiary()
        {
            Students = new ObservableCollection<StudentWrapper>(
                _repository.GetStudents(SelectedGroupId));
        }

        private void ConnectionSettings(object obj)
        {
            var connectionSettingsView = new ConnectionSettingsView(true);
            connectionSettingsView.ShowDialog();
        }
    }
}

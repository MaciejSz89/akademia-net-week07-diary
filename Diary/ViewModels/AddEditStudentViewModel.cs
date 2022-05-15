using Diary.Commands;
using Diary.Models.Wrappers;
using Diary.Models;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Diary.Models.Domains;

namespace Diary.ViewModels
{
    public class AddEditStudentViewModel : ViewModelBase
    {
        public AddEditStudentViewModel(StudentWrapper student = null)
        {


            if (student == null)
                Student = new StudentWrapper();
            else
            {
                Student = student;
                IsUpdate = true;
            }

            ConfirmCommand = new RelayCommand(Confirm);
            CloseCommand = new RelayCommand(Close);
            InitGroups();
        }

        public ICommand ConfirmCommand { get; set; }
        public ICommand CloseCommand { get; set; }


        private Repository _repository = new Repository();


        private StudentWrapper _student;
        public StudentWrapper Student
        {
            get
            {
                return _student;
            }
            set
            {
                _student = value;
                OnPropertyChanged();
            }
        }


        private bool _isUpdate = false;

        public bool IsUpdate
        {
            get
            {
                return _isUpdate;
            }
            set
            {
                _isUpdate = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Group> Groups { get; set; }

        private void Close(object obj)
        {
            CloseWindow(obj as Window);
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }

        private void Confirm(object obj)
        {
            if (!Student.IsValid)
                return;

            if(!IsUpdate)
            {
                AddStudent();
            }
            else
            {
                UpdateStudent();
            }
            CloseWindow(obj as Window);
        }
        private void AddStudent()
        {
            _repository.AddStudent(Student);
        }

        private void UpdateStudent()
        {
            _repository.UpdateStudent(Student);
        }



        private void InitGroups()
        {


            var groups = _repository.GetGroups();
            groups.Insert(0, new Group { Id = 0, Name = "--brak--" });

            Groups = new ObservableCollection<Group>(groups);

        }
    }
}

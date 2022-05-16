using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary
{
    public class ConnectionSettings : IDataErrorInfo
    {

        private bool _isServerAddressValid;
        private bool _isServerNameValid;
        private bool _isDatabaseNameValid;
        private bool _isUserIdValid;
        private bool _isPasswordValid;

        public string ServerAddress
        {
            get
            {
                return Properties.Settings.Default.ServerAddress;
            }
            set
            {
                Properties.Settings.Default.ServerAddress = value;
            }
        }
        public string ServerName
        {
            get
            {
                return Properties.Settings.Default.ServerName;
            }
            set
            {
                Properties.Settings.Default.ServerName = value;
            }
        }
        public string DatabaseName
        {
            get
            {
                return Properties.Settings.Default.DatabaseName;
            }
            set
            {
                Properties.Settings.Default.DatabaseName = value;
            }
        }


        public string UserId
        {
            get
            {
                return Properties.Settings.Default.UserId;
            }
            set
            {
                Properties.Settings.Default.UserId = value;
            }
        }
        public string Password
        {
            get
            {
                return Properties.Settings.Default.Password;
            }
            set
            {
                Properties.Settings.Default.Password = value;
            }
        }

        public string Error { get; set; }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(ServerAddress):
                        if (string.IsNullOrWhiteSpace(ServerAddress))
                        {
                            Error = "Pole Adres serwera jest wymagane.";
                            _isServerAddressValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isServerAddressValid = true;
                        }
                        break;

                    case nameof(ServerName):
                        if (string.IsNullOrWhiteSpace(ServerName))
                        {
                            Error = "Pole Nazwa serwera jest wymagane.";
                            _isServerNameValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isServerNameValid = true;
                        }
                        break;

                    case nameof(DatabaseName):
                        if (string.IsNullOrWhiteSpace(DatabaseName))
                        {
                            Error = "Pole Baza danych jest wymagane.";
                            _isDatabaseNameValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isDatabaseNameValid = true;
                        }
                        break;

                    case nameof(UserId):
                        if (string.IsNullOrWhiteSpace(UserId))
                        {
                            Error = "Pole Użytkownik jest wymagane.";
                            _isUserIdValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isUserIdValid = true;
                        }
                        break;

                    case nameof(Password):
                        if (string.IsNullOrWhiteSpace(Password))
                        {
                            Error = "Pole Hasło jest wymagane.";
                            _isPasswordValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isPasswordValid = true;
                        }
                        break;

                    default:
                        break;
                }
                return Error;
            }
        }


        public bool Save()
        {
            if (!IsConnectionAvailable())
                return false;

            Properties.Settings.Default.Save();
            return true;
        }

        public bool IsConnectionAvailable()
        {
            var connectionString = $@"Server={ServerAddress}\{ServerName};User Id={UserId};Password={Password};";

            return IsConnectionAvailable(connectionString);
        }

        public bool IsConnectionAvailable(string connectionString)
        {


            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception)
                {
                    return false;
                }
                connection.Close();
            }

            return true;
        }

        public bool IsValid
        {
            get
            {
                return _isServerAddressValid && _isServerNameValid && _isDatabaseNameValid && _isUserIdValid && _isPasswordValid;
            }
        }
    }
}

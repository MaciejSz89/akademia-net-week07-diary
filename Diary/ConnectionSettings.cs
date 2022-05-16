using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary
{
    public class ConnectionSettings
    {
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
    }
}

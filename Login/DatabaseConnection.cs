﻿using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Login
{
    class DatabaseConnection
    {
        private MySqlConnection sqlConnection;
        private String connection_string;
        public DatabaseConnection()
        {
            String db_host = "localhost";
            String db_name = "users";
            String db_username = "root";
            String db_password = "anbalagan";
            connection_string = "SERVER="+db_host+";DATABASE=" + db_name + 
                ";UID=" + db_username + ";PASSWORD=" + db_password + ";";            
        }

        public void connect()
        {
            sqlConnection = new MySqlConnection(connection_string);
            try
            {
                sqlConnection.Open();
            }catch(MySqlException e)
            {
                switch (e.Number)
                {
                    case 0:
                        MessageBox.Show("Unable to connect to database.");
                        break;
                    case 1045:
                        MessageBox.Show("Wrong database username or password.");
                        break;
                }
            }
        }

        public Boolean verifyUser(String uname, String pass)
        {
            String query = "SELECT passwd FROM login_data WHERE user_name = '" + uname + "';";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            String p="lol";
            while (dataReader.Read())
                p = dataReader[0].ToString();
            dataReader.Close();
            if (p.Equals(pass))
                return true;
            else
                return false;
        }

        ~DatabaseConnection()
        {
            sqlConnection.Close();
        }
    }
}

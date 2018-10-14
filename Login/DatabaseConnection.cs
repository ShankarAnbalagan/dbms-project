using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MockSAP
{
    public class DatabaseConnection
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

        public Boolean verifyUser(String AdminPass)
        {
            String query = "SELECT passwd FROM login_data WHERE user_name = 'ADMIN';";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            String p = "lol";
            while (dataReader.Read())
                p = dataReader[0].ToString();
            dataReader.Close();
            if (p.Equals(AdminPass))
                return true;
            else
                return false;
        }

        public String getUserId(String uname)
        {
            String query = "SELECT user_id FROM login_data WHERE user_name = '" + uname + "';";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            String p = "lol";
            while (dataReader.Read())
                p = dataReader[0].ToString();
            dataReader.Close();
            return p;
        }

        public Boolean addUser(String uid, String uname, String upass)
        {
            String query = "INSERT INTO login_data VALUES("+uid+",'"+uname+"','"+upass+"');";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            try
            {
                mySqlCommand.ExecuteNonQuery();
            }catch(MySqlException e)
            {
                if (e.Number == 1062)
                {
                    MessageBox.Show("User already exists");
                }
                else
                    MessageBox.Show(e.ToString());
                return false;
            }
            return true;
        }

        ~DatabaseConnection()
        {
            if(sqlConnection!=null)
                sqlConnection.Close();
        }
    }
}

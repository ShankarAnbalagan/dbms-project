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
            String db_name = "mocksap";
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

        public Boolean removeUser(String uid, String upass)
        {
            String query = "DELETE FROM login_data WHERE user_id='" + uid + "' AND passwd='" + upass + "';";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            try
            {
                mySqlCommand.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (e.Number == 1062)
                {
                    MessageBox.Show("User ddes not exit");
                }
                else
                    MessageBox.Show(e.ToString());
                return false;
            }
            return true;
        }

        public List<String[]> getVendors()
        {
            List<String[]> l = new List<String[]>();
            String query = "SELECT * FROM vendor";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                String[] s = {
                    dataReader[0].ToString(),dataReader[1].ToString(),dataReader[2].ToString(),dataReader[3].ToString()};
                l.Add(s);
            }
            dataReader.Close();
            return l;
        }

        public List<String[]> getPurchases()
        {
            List<String[]> l = new List<String[]>();
            String query = "SELECT purchase_id, material_id, vendor_id, quantity, cost, DATE_FORMAT(date_of_purchase,'%y-%m-%d')" +
                " FROM purchases;";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                String[] s = {
                    dataReader[0].ToString(),dataReader[1].ToString(),dataReader[2].ToString(),dataReader[3].ToString(),dataReader[4].ToString(),dataReader[5].ToString()};
                l.Add(s);
            }
            dataReader.Close();
            return l;
        }

        public List<String> getMaterialNames()
        {
            List<String> l = new List<String>();
            String query = "SELECT material_name FROM material;";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                l.Add(dataReader[0].ToString());
            }
            dataReader.Close();
            return l;
        }

        public List<String> getVendorNames()
        {
            List<String> l = new List<String>();
            String query = "SELECT vendor_name,vendor_id FROM vendor;";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                l.Add((dataReader[0].ToString()+","+dataReader[1]));
            }
            dataReader.Close();
            return l;
        }

        public void NewPurchase(String purchase_id, String material_name, String vendor_id, String quantity, String cost, String[] date)
        {
            String query = "SELECT material_id FROM material WHERE material_name='"+material_name+"';";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            String material_id = "";
            while (dataReader.Read())
            {
                material_id = dataReader[0].ToString();
            }
            dataReader.Close();
            query = "INSERT INTO purchases VALUES('"+purchase_id+"','"+material_id+"','"+vendor_id+"',"+quantity+","+cost+",'"+date[2]+"-"+date[1]+"-"+date[0]+"');";
            mySqlCommand = new MySqlCommand(query, sqlConnection);
            mySqlCommand.ExecuteNonQuery();
            MessageBox.Show("Success");
        }

        ~DatabaseConnection()
        {
            if(sqlConnection!=null)
                sqlConnection.Close();
        }
    }
}

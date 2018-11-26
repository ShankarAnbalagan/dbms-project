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
            String query = "call get_adminpass();";     //Calling stored procedure
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
                    MessageBox.Show("User does not exit");
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
            String query = "SELECT * FROM vendor;";
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

        public String[] getVendors(String vid)
        {
            String query = "SELECT * FROM vendor WHERE vendor_id = '"+vid+"';";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            String[] s = null;
            while (dataReader.Read())
            {
                s = new String[] {
                    dataReader[0].ToString(),dataReader[1].ToString(),dataReader[2].ToString(),dataReader[3].ToString()};
            }
            dataReader.Close();
            return s;
        }

        public String[] getBuyer(String vid)
        {
            String query = "SELECT * FROM buyer WHERE buyer_id = '" + vid + "';";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            String[] s = null;
            while (dataReader.Read())
            {
                s = new String[] {
                    dataReader[0].ToString(),dataReader[1].ToString(),dataReader[2].ToString(),dataReader[3].ToString()};
            }
            dataReader.Close();
            return s;
        }

        public List<String[]> getPurchases()
        {
            List<String[]> l = new List<String[]>();
            String query = "SELECT purchase_id, material_id, vendor_id, quantity, cost, DATE_FORMAT(date_of_purchase,'%d-%m-%y')" +
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

        public String getProductCost(String prodid)
        {
            String s="";
            String query = "SELECT cost FROM manufacturing WHERE product_id='" + prodid + "';";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                s = dataReader[0].ToString();
            }
            dataReader.Close();
            return s;
        }

        public List<String> getProductlNames()
        {
            List<String> l = new List<String>();
            String query = "SELECT product_name FROM manufacturing;";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                l.Add(dataReader[0].ToString());
            }
            dataReader.Close();
            return l;
        }

        public List<String> getManufactureProductslNames()
        {
            List<String> l = new List<String>();
            String query = "SELECT product_name FROM manufacturing;";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                l.Add(dataReader[0].ToString());
            }
            dataReader.Close();
            return l;
        }

        public String getProductId(String prodname)
        {
            String s = "";
            String query = "SELECT product_id FROM manufacturing where product_name='"+prodname+"';";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                s=dataReader[0].ToString();
            }
            dataReader.Close();
            return s;
        }

        public Dictionary<String,int> getManufacturingData(String id)
        {
            Dictionary<String, int> data = new Dictionary<String, int>();
            String query = "SELECT Aluminium, Copper, Ceramic, Iron, Plastic, Silicon, Silver, Steel FROM manufacturing where product_id='" + id + "';";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                data.Add("Aluminium", Convert.ToInt32(dataReader[0].ToString()));
                data.Add("Copper", Convert.ToInt32(dataReader[1].ToString()));
                data.Add("Ceramic", Convert.ToInt32(dataReader[2].ToString()));
                data.Add("Iron", Convert.ToInt32(dataReader[3].ToString()));
                data.Add("Plastic", Convert.ToInt32(dataReader[4].ToString()));
                data.Add("Silicon", Convert.ToInt32(dataReader[5].ToString()));
                data.Add("Silver", Convert.ToInt32(dataReader[6].ToString()));
                data.Add("Steel", Convert.ToInt32(dataReader[7].ToString()));
            }
            dataReader.Close();
            return data;
        }

        public Dictionary<String,int> getQuantityFromMaterials(List<String> l)
        {
            String query = "";
            MySqlCommand mySqlCommand;
            MySqlDataReader dataReader;
            Dictionary<String, int> data = new Dictionary<string, int>();
            foreach (String s in l)
            {
                query = "SELECT total_quantity FROM material WHERE material_name='" + s + "';";
                mySqlCommand = new MySqlCommand(query, sqlConnection);
                dataReader = mySqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    data.Add(s,Convert.ToInt32(dataReader[0].ToString()));
                }
                dataReader.Close();
            }
            return data;
        }

        public Boolean Manufacture(String manid, String prodid, Dictionary<String, int> data, Dictionary<String, int> reqdata, String[] date)
        {
            String query;
            MySqlCommand mySqlCommand;
            Dictionary<String, int> updatedata = new Dictionary<string, int>();
            int n;
            foreach (KeyValuePair<String, int> keyValuePair in data)
            {
                reqdata.TryGetValue(keyValuePair.Key, out n);
                updatedata.Add(keyValuePair.Key, (n - keyValuePair.Value));
            }
            foreach(KeyValuePair<String,int> keyValuePair in updatedata)
            {
                query = "UPDATE material SET total_quantity = " + keyValuePair.Value + " WHERE material_name ='" + keyValuePair.Key + "';";
                mySqlCommand = new MySqlCommand(query, sqlConnection);
                mySqlCommand.ExecuteNonQuery();
            }

            query = "INSERT INTO products VALUE('" + manid + "','" + prodid + "','" + getProductCost(prodid) + "','" + date[2] + "-" + date[1] + "-" + date[0] + "');";
            try
            {
                mySqlCommand = new MySqlCommand(query, sqlConnection);
                mySqlCommand.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (e.Number == 1062)
                {
                    MessageBox.Show("Manufacture ID already exists");
                }
                else
                    MessageBox.Show(e.ToString());
                return false;
            }
            return true;
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

        public List<String> getBuyerNames()
        {
            List<String> l = new List<String>();
            String query = "SELECT buyer_name,buyer_id FROM buyer;";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                l.Add((dataReader[0].ToString() + "," + dataReader[1]));
            }
            dataReader.Close();
            return l;
        }

        public Boolean NewPurchase(String purchase_id, String material_name, String vendor_id, String quantity, String cost, String[] date)
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
            try
            {
                mySqlCommand = new MySqlCommand(query, sqlConnection);
                mySqlCommand.ExecuteNonQuery();
            }catch(MySqlException e)
            {
                if (e.Number == 1062)
                {
                    MessageBox.Show("Purchase ID already exists");
                }
                else
                    MessageBox.Show(e.ToString());
                return false;
            }

            query = "SELECT total_quantity FROM material WHERE material_id='" + material_id + "';";
            String material_quantity = "";
            mySqlCommand = new MySqlCommand(query, sqlConnection);
            dataReader = mySqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                material_quantity = dataReader[0].ToString();
            }
            dataReader.Close();

            double quan = Convert.ToDouble(material_quantity) + Convert.ToDouble(quantity);
            query = "UPDATE material SET total_quantity = "+quan+" WHERE material_id ='"+material_id+"';";
            mySqlCommand = new MySqlCommand(query, sqlConnection);
            mySqlCommand.ExecuteNonQuery();
            return true;
        }

        public Boolean NewSale(String shipping_id, String product_name, String buyer_id, String cost, String[] date)
        {

            String query = "SELECT product_id FROM manufacturing WHERE product_name='" + product_name + "';";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            String product_id = "";
            while (dataReader.Read())
            {
                product_id = dataReader[0].ToString();
            }
            dataReader.Close();

            query = "SELECT COUNT(*) FROM products WHERE product_id='" + product_id + "';";
            mySqlCommand = new MySqlCommand(query, sqlConnection);
            int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
            if(count == 0)
            {
                return false;                
            }

            query = "SELECT manufacture_id FROM products where product_id='" + product_id + "' ORDER BY RAND() LIMIT 0,1;";
            mySqlCommand = new MySqlCommand(query, sqlConnection);
            dataReader = mySqlCommand.ExecuteReader();
            String manufacture_id = "";
            while (dataReader.Read())
            {
                manufacture_id = dataReader[0].ToString();
            }
            dataReader.Close();

            query = "INSERT INTO shipping VALUES('" + shipping_id + "','" + manufacture_id + "','" + product_id + "','" + buyer_id + "'," + cost + ",'" + date[2] + "-" + date[1] + "-" + date[0] + "');";
            try
            {
                mySqlCommand = new MySqlCommand(query, sqlConnection);
                mySqlCommand.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (e.Number == 1062)
                {
                    MessageBox.Show("Sales ID already exists");
                }
                else
                    MessageBox.Show(e.ToString());
                return false;
            }

            query = "DELETE FROM products WHERE manufacture_id='" + manufacture_id + "';";
            mySqlCommand = new MySqlCommand(query, sqlConnection);
            mySqlCommand.ExecuteNonQuery();

            return true;
        }

        public Boolean NewVendor(String vid, String vname, String vaddr, String vphone)
        {
            String query = "INSERT INTO vendor VALUES('"+vid+"','"+vname+"','"+vaddr+"','"+vphone+"');";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            try
            {
                mySqlCommand.ExecuteNonQuery();
            }catch(MySqlException e)
            {
                if (e.Number == 1062)
                {
                    MessageBox.Show("Vendor already exists");
                    return false;
                }
                else
                    MessageBox.Show(e.ToString());
            }
            return true;
        }

        public Boolean NewBuyer(String vid, String vname, String vaddr, String vphone)
        {
            String query = "INSERT INTO buyer VALUES('" + vid + "','" + vname + "','" + vaddr + "','" + vphone + "');";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            try
            {
                mySqlCommand.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (e.Number == 1062)
                {
                    MessageBox.Show("Buyer already exists");
                    return false;
                }
                else
                    MessageBox.Show(e.ToString());
            }
            return true;
        }

        public void DeleteVendor(String vid)
        {
            String query = "DELETE FROM vendor WHERE vendor_id='" + vid + "';";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            mySqlCommand.ExecuteNonQuery();
        }

        public void DeleteBuyer(String vid)
        {
            String query = "DELETE FROM buyer WHERE buyer_id='" + vid + "';";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            mySqlCommand.ExecuteNonQuery();
        }

        public Boolean ModifyVendor(String vid, String vname, String vaddr, String vphone)
        {
            String query = "UPDATE vendor SET vendor_name='"+ vname + "', vendor_address='" + vaddr + "', vendor_phone='" + vphone + "' WHERE vendor_id='"+ vid +"';";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            try
            {
                mySqlCommand.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (e.Number == 1062)
                {
                    MessageBox.Show("Vendor already exists");
                    return false;
                }
                else
                    MessageBox.Show(e.ToString());
            }
            return true;
        }

        public Boolean ModifyBuyer(String vid, String vname, String vaddr, String vphone)
        {
            String query = "UPDATE buyer SET buyer_name='" + vname + "', buyer_address='" + vaddr + "', buyer_phone='" + vphone + "' WHERE buyer_id='" + vid + "';";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            try
            {
                mySqlCommand.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (e.Number == 1062)
                {
                    MessageBox.Show("Buyer already exists");
                    return false;
                }
                else
                    MessageBox.Show(e.ToString());
            }
            return true;
        }

        public List<String[]> GetMaterial()
        {
            List<String[]> l = new List<String[]>();
            String query = "SELECT * FROM material;";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                String[] s = {
                    dataReader[1].ToString(),dataReader[2].ToString()};
                l.Add(s);
            }
            dataReader.Close();
            return l;
        }

        public List<String[]> getBuyers()
        {
            List<String[]> l = new List<String[]>();
            String query = "SELECT * FROM buyer;";
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

        public String[] getBuyers(String vid)
        {
            String query = "SELECT * FROM buyer WHERE buyer_id = '" + vid + "';";
            MySqlCommand mySqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
            String[] s = null;
            while (dataReader.Read())
            {
                s = new String[] {
                    dataReader[0].ToString(),dataReader[1].ToString(),dataReader[2].ToString(),dataReader[3].ToString()};
            }
            dataReader.Close();
            return s;
        }

        public List<String[]> getSales()
        {
            List<String[]> l = new List<String[]>();
            String query = "SELECT shipping_id, manufacture_id, product_id, buyer_id, cost, DATE_FORMAT(date_of_shipping,'%d-%m-%y')" +
                " FROM shipping;";
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

        public List<String[]> getProducts()
        {
            List<String[]> l = new List<String[]>();
            String query = "SELECT manufacture_id, product_id, manufacturing_cost, DATE_FORMAT(date_of_manufacture,'%d-%m-%y')" +
                " FROM products;";
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

        ~DatabaseConnection()
        {
            if(sqlConnection!=null)
                sqlConnection.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MockSAP
{
    /// <summary>
    /// Interaction logic for Manufacture.xaml
    /// </summary>
    public partial class Manufacture : Window
    {
        DatabaseConnection databaseConnection;
        HomePage homePage;
        public Manufacture()
        {
            InitializeComponent();
            error.Text = "";
        }

        public void StartPage(DatabaseConnection connection, HomePage h)
        {
            homePage = h;
            databaseConnection = connection;
            this.Show();
        }

        private void manufacture_Click(object sender, RoutedEventArgs e)
        {
            String manid = manufacture_id.Text;
            String prod = product_dropdown.Text;
            String prodid = databaseConnection.getProductId(prod);
            String dat = date_of_manufacture.Text;
            if (verify(manid, dat))
            {
                String[] date = dat.Split('-');
                Dictionary<String, int> metadata = databaseConnection.getManufacturingData(prodid);
                Dictionary<String, int> data = new Dictionary<string, int>();
                Dictionary<String, int> reqdata = new Dictionary<string, int>();
                List<String> materialsRequired = new List<string>();
                foreach (KeyValuePair<String, int> keyValuePair in metadata)
                {
                    if (keyValuePair.Value != 0)
                    {
                        data.Add(keyValuePair.Key, keyValuePair.Value);
                        materialsRequired.Add(keyValuePair.Key);
                    }
                }
                reqdata = databaseConnection.getQuantityFromMaterials(materialsRequired);
                int n;
                foreach (KeyValuePair<String, int> keyValuePair in data)
                {
                    reqdata.TryGetValue(keyValuePair.Key, out n);
                    if (n - keyValuePair.Value <= 0)
                    {
                        MessageBox.Show("Insufficient materials to manufacture product");
                        this.Close();
                        return;
                    }
                }
                if (databaseConnection.Manufacture(manid, prodid, data, reqdata, date))
                {
                    homePage.RefreshProductList();
                    this.Close();
                    return;
                }
            }
        }

        private Boolean verify(String id, String date)
        {
            if(!((id.Length <= 4 && id.Length > 0) && date.Length > 0))
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
                error.Text = "Incorrect entries. Enter values again.";
                return false;
            }
            return true;
        }

        private void product_dropdown_Loaded(object sender, RoutedEventArgs e)
        {
            List<String> l = databaseConnection.getManufactureProductslNames();
            product_dropdown.ItemsSource = l;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            homePage.IsEnabled = true;
        }
    }
}

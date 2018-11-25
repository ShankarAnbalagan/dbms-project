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
    /// Interaction logic for NewSale.xaml
    /// </summary>
    public partial class NewSale : Window
    {
        DatabaseConnection databaseConnection;
        HomePage homePage;
        public NewSale()
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

        private void product_dropdown_Loaded(object sender, RoutedEventArgs e)
        {
            List<String> l = databaseConnection.getProductlNames();
            product_dropdown.ItemsSource = l;
        }

        private void buyer_dropdown_Loaded(object sender, RoutedEventArgs e)
        {
            List<String> l = databaseConnection.getBuyerNames();
            buyer_dropdown.ItemsSource = l;
        }

        private void make_sale_Click(object sender, RoutedEventArgs e)
        {
            error.Foreground = new SolidColorBrush(Colors.Red);
            String pid = sale_id.Text.Trim();
            String mat = product_dropdown.Text.Trim();
            String temp_ven = buyer_dropdown.Text.Trim();
            String ven = "";
            try
            {
                String[] temp_split = temp_ven.Split(',');
                ven = temp_split[1];
            }
            catch (Exception)
            {
                error.Text = "Blank entry. Enter values again.";
            }
            String cos = cost.Text;
            String dat = dateofsale.Text;
            if (VerifyInput(pid, cos, dat))
            {
                String[] date = dat.Split('-');
                if (databaseConnection.NewSale(pid, mat, ven, cos, date))
                {
                    MessageBox.Show("New Sale Added");
                    homePage.RefreshSalesList();
                    homePage.IsEnabled = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Product not in stock");
                    this.Close();
                }
            }
        }

        private Boolean VerifyInput(String pid, String cos, String date)
        {
            if (!((pid.Length <= 4 && pid.Length > 0) && IsDigitsOnly(cos) && date.Length>0))
            {
                error.Text = "Incorrect entries. Enter values again.";
                return false;
            }
            return true;
        }

        private Boolean IsDigitsOnly(String str)
        {
            String[] s = str.Split('.');
            str = "";
            foreach (String i in s)
            {
                str += i;
            }
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] < '0' || str[i] > '9')
                    return false;
            }

            return true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            homePage.IsEnabled = true;
        }
    }
}

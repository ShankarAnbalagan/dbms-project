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
    /// Interaction logic for NewPurchase.xaml
    /// </summary>
    public partial class NewPurchase : Window
    {
        DatabaseConnection databaseConnection;
        HomePage homePage;
        public NewPurchase()
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

        private void materials_dropdown_Loaded(object sender, RoutedEventArgs e)
        {

            List<String> l = databaseConnection.getMaterialNames();
            materials_dropdown.ItemsSource = l;
        }

        private void vendor_dropdown_Loaded(object sender, RoutedEventArgs e)
        {
            List<String> l = databaseConnection.getVendorNames();
            vendor_dropdown.ItemsSource = l;
        }

        private void make_purchase_Click(object sender, RoutedEventArgs e)
        {
            error.Foreground = new SolidColorBrush(Colors.Red);
            String pid = purchase_id.Text.Trim();
            String mat = materials_dropdown.Text.Trim();
            String temp_ven = vendor_dropdown.Text.Trim();
            String ven = "";
            try
            {
                String[] temp_split = temp_ven.Split(',');
                ven = temp_split[1];
            } catch(Exception)
            {
                error.Text = "Blank entry. Enter values again.";
            }
            String quan = quantity.Text;
            String cos = cost.Text;
            String dat = dateofpurchase.Text;
            if (VerifyInput(pid, quan, cos))
            {
                String[] date = dat.Split('-');
                if(databaseConnection.NewPurchase(pid, mat, ven, quan, cos, date))
                {
                    MessageBox.Show("New purchase Added");
                    homePage.RefreshPurchaseList();
                    homePage.IsEnabled = true;
                    this.Close();
                }
            }            
        }

        private Boolean VerifyInput(String pid, String quan, String cos)
        {
            if(!((pid.Length<=4 && pid.Length > 0) && IsDigitsOnly(quan) && IsDigitsOnly(cos)))
            {
                error.Text="Incorrect entries. Enter values again.";
                return false;
            }
            return true;
        }

        private Boolean IsDigitsOnly(String str)
        {
            String[] s = str.Split('.');
            str = "";
            foreach(String i in s)
            {
                str +=i;
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

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
    /// Interaction logic for NewVendor.xaml
    /// </summary>
    public partial class NewVendor : Window
    {
        DatabaseConnection databaseConnection;
        HomePage homePage;
        public NewVendor()
        {
            InitializeComponent();
        }

        public void StartPage(DatabaseConnection connection, HomePage h)
        {
            homePage = h;
            databaseConnection = connection;
            this.Show();
        }

        private void add_vendor_Click(object sender, RoutedEventArgs e)
        {
            String vendorID = vendor_id.Text.Trim();
            String vendorName = vendor_name.Text.Trim();
            String vendorAddress = vendor_address.Text.Trim();
            String vendorPhone = vendor_phone.Text.Trim();

            if (vendorID.Length != 0 && vendorName.Length != 0 && vendorAddress.Length != 0 && vendorPhone.Length != 0)
            {

                if (databaseConnection.NewVendor(vendorID, vendorName, vendorAddress, vendorPhone))
                {
                    MessageBox.Show("New Vendor Added");
                    homePage.RefreshVendorList();
                }
                homePage.IsEnabled = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Blank entries. Please enter Data");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            homePage.IsEnabled = true;
        }
    }
}

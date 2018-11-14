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
    /// Interaction logic for ModifyVendor.xaml
    /// </summary>
    public partial class ModifyVendor : Window
    {
        DatabaseConnection databaseConnection;
        HomePage homePage;
        public ModifyVendor()
        {
            InitializeComponent();
        }

        public void StartPage(DatabaseConnection connection,HomePage homePage,String vid)
        {
            databaseConnection = connection;
            this.homePage = homePage;
            String[] s = databaseConnection.getVendors(vid);
            vendor_id.Text = s[0];
            vendor_name.Text = s[1];
            vendor_address.Text = s[2];
            vendor_phone.Text = s[3];
            this.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            homePage.IsEnabled=true;
        }

        private void modify_vendor_Click(object sender, RoutedEventArgs e)
        {
            String vendorID = vendor_id.Text.Trim();
            String vendorName = vendor_name.Text.Trim();
            String vendorAddress = vendor_address.Text.Trim();
            String vendorPhone = vendor_phone.Text.Trim();

            if (vendorID.Length != 0 && vendorName.Length != 0 && vendorAddress.Length != 0 && vendorPhone.Length != 0)
            {

                if (databaseConnection.ModifyVendor(vendorID, vendorName, vendorAddress, vendorPhone))
                {
                    MessageBox.Show("Vendor Data Modified");
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
    }
}

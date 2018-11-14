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
    /// Interaction logic for ModifyBuyer.xaml
    /// </summary>
    public partial class ModifyBuyer : Window
    {
        DatabaseConnection databaseConnection;
        HomePage homePage;
        public ModifyBuyer()
        {
            InitializeComponent();
        }

        public void StartPage(DatabaseConnection connection, HomePage homePage, String vid)
        {
            databaseConnection = connection;
            this.homePage = homePage;
            String[] s = databaseConnection.getBuyer(vid);
            buyer_id.Text = s[0];
            buyer_name.Text = s[1];
            buyer_address.Text = s[2];
            buyer_phone.Text = s[3];
            this.Show();
        }

        private void modify_buyer_Click(object sender, RoutedEventArgs e)
        {
            String buyerID = buyer_id.Text.Trim();
            String buyerName = buyer_name.Text.Trim();
            String buyerAddress = buyer_address.Text.Trim();
            String buyerPhone = buyer_phone.Text.Trim();

            if (buyerID.Length != 0 && buyerName.Length != 0 && buyerAddress.Length != 0 && buyerPhone.Length != 0)
            {

                if (databaseConnection.ModifyBuyer(buyerID, buyerName, buyerAddress, buyerPhone))
                {
                    MessageBox.Show("Buyer Data Modified");
                    homePage.RefreshBuyerList();
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

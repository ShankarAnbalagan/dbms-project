using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class HomePage : Window
    {
        private DatabaseConnection database;
        private DataTable purchaseTable;
        private DataTable vendorTable;
        private VendorList vendorList;
        private PurchasesList purchasesList;
        private NewPurchase newPurchase;
        private NewVendor newVendor;
        private ModifyVendor modifyVendor;
        public HomePage()
        {
            InitializeComponent();
        }

        public void StartPage(String uname, DatabaseConnection database)
        {
            this.database = database;
            this.Show();
            Loggedon_name.Text = "User ID : "+this.database.getUserId(uname)+"   Username : " + uname;
            purchaseTable = new DataTable();
            purchasesList = new PurchasesList(purchaseTable, database);
            vendorTable = new DataTable();
            vendorList = new VendorList(vendorTable, database);
            PopulateGridViews();
        }

        private void PopulateGridViews()
        {
            VendorListGrid.ItemsSource = vendorList.AddColums().DefaultView;
            VendorListGrid.ItemsSource = vendorList.AddRows().DefaultView;
            PurchasesGrid.ItemsSource = purchasesList.AddColums().DefaultView;
            PurchasesGrid.ItemsSource = purchasesList.AddRows().DefaultView;
        }

        private void Logout_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void new_purchase_Click(object sender, RoutedEventArgs e)
        {
            newPurchase = new NewPurchase();
            this.IsEnabled = false;
            newPurchase.StartPage(this.database,this);
        }

        public void RefreshPurchaseList()
        {
            purchaseTable.Clear();
            PurchasesGrid.ItemsSource = purchasesList.AddRows().DefaultView;
        }

        public void RefreshVendorList()
        {
            vendorTable.Clear();
            VendorListGrid.ItemsSource =vendorList.AddRows().DefaultView;
        }

        private void new_vendor_Click(object sender, RoutedEventArgs e)
        {
            newVendor = new NewVendor();
            this.IsEnabled = false;
            newVendor.StartPage(this.database,this);
        }

        private void modify_vendor_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = VendorListGrid.SelectedItem as DataRowView;
            if (item == null)
                MessageBox.Show("No Vendor Selected");
            else
            {
                modifyVendor = new ModifyVendor();
                this.IsEnabled = false;
                modifyVendor.StartPage(database,this, item.Row[0].ToString());
            }
        }        

        private void delete_vendor_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = VendorListGrid.SelectedItem as DataRowView;
            if (item == null)
                MessageBox.Show("No Vendor Selected");
            else
            {
                MessageBoxResult result= MessageBox.Show("Are you sure you want to remove vendor with ID "
                    +item.Row[0].ToString()+" ?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    database.DeleteVendor(item.Row[0].ToString());
                    RefreshVendorList();
                    MessageBox.Show("Vendor Successfully Deleted");
                }
                else
                {
                    return;
                }
            }
        }
    }
}

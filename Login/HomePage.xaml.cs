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
        private DataTable vendorListTable;
        private VendorList vendorList;
        private PurchasesList purchasesList;
        private NewPurchase newPurchase;
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
            vendorListTable = new DataTable();
            vendorList = new VendorList(vendorListTable, database);
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
            newPurchase.StartPage(this.database);
        }
    }
}
